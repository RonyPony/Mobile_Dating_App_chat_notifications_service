using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Hubs
{
    /// <summary>
    /// Represents a Hub to the app.
    /// </summary>
    public class MessengerHub : Hub
    {
        #region Fields
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        private readonly IRoomService _roomService;
        private readonly IChatRoomMessageService _chatRoomMessageService;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates an instance of <see cref="MessengerHub"/>
        /// </summary>
        /// <param name="clientService">An implementation of <see cref="IClientService"/>.</param>
        /// <param name="roomService">An implementation of <see cref="IRoomService"/>.</param>
        /// <param name="mapper">An implementation of <see cref="IMapper"/>.</param>
        /// <param name="chatRoomMessageService">An implementation of <see cref="IChatRoomMessageService"/>.</param>
        public MessengerHub(IClientService clientService , IRoomService roomService , 
            IMapper mapper , IChatRoomMessageService chatRoomMessageService)
        {
            _clientService = clientService;
            _roomService = roomService;
            _mapper = mapper;
            _chatRoomMessageService = chatRoomMessageService;
        }

        #endregion

        #region Methods
        /// <summary>
        ///  Signalr method to tracking when the client is connected.
        /// </summary>
        public override Task OnConnectedAsync()
        {
            string currentClientConnectionId = Context.ConnectionId;
            _clientService.ClientIsConnected(currentClientConnectionId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Signalr method to tracking when the client is disconnected.
        /// </summary>
        /// <param name="exception">exception</param>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string currentClientConnectionId = Context.ConnectionId;
            _clientService.ClientIsDisconnected(currentClientConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Opens the channel for communication between clients
        /// </summary>
        /// <param name="registerToUserDto">registerToUserDto request</param>
        public async Task<string> OpenRoom(ClientToRegisterRequest registerToUserDto)
        {
            if (registerToUserDto is null)
                throw new HubException("InvalidUserRequest");

            Client registeredUser = await _clientService.GetClientByUsername(registerToUserDto.Username);

            Room foundRoom = await _roomService.GetRoomByName(registerToUserDto.RoomName);

            if (registeredUser is null)
            {
                await RegisterClient(registerToUserDto, foundRoom);
            }
            else
            {
                await UserJoinToRoom(registeredUser, registerToUserDto.RoomName , registerToUserDto.OrderId);
            }

            IList<ChatRoomMessageModel> roomMessages = _chatRoomMessageService
                .GetMessageOfRoomByRoomId(foundRoom.Id)
                .Select(select => new ChatRoomMessageModel
                {
                    UserName = _clientService.GetClientById(select.ClientId).Username ?? "N/A",
                    ClientId = select.ClientId,
                    RoomId = select.RoomId,
                    MessageContent = select.MessageContent,
                    CreatedOnUtc = select.CreatedOnUtc,
                }).ToList();

            return JsonConvert.SerializeObject(roomMessages);
        }

        /// <summary>
        /// Send the message to the Room.
        /// </summary>
        /// <param name="orderId">order's assigned to room</param>
        /// <param name="message">message write for the client</param>
        public async Task SendMessage(SendMessageModel messageModel)
        {
            if (messageModel is null)
                throw new HubException("Invalid message request.");

            Room foundRoom = await _roomService.GetRoomByOrderId(messageModel.OrderId);

            if (foundRoom is null)
                throw new HubException("RoomNotFound");

            Client foundClient = await _clientService.GetClientByUsername(messageModel.Username);

            if (foundClient is null)
                throw new HubException("ClientNotFound");

            await _chatRoomMessageService.InsertChatRoomMessage(new ChatRoomMessages
            {
                ClientId = foundClient.Id,
                RoomId = foundRoom.Id,
                MessageContent = messageModel.MessageContent,
                CreatedOnUtc = DateTime.Now
            });

            IList<ChatRoomMessageModel> roomMessages = _chatRoomMessageService
                .GetMessageOfRoomByRoomId(foundRoom.Id)
                .Select(select => new ChatRoomMessageModel {
                   UserName = _clientService.GetClientById(select.ClientId).Username ?? "N/A",
                   ClientId = select.ClientId,
                   RoomId = select.RoomId,
                   MessageContent = select.MessageContent,
                   CreatedOnUtc = select.CreatedOnUtc,
                }).ToList();

            string messagesContent = JsonConvert.SerializeObject(roomMessages);

            await Clients.Group(foundRoom.Name).SendAsync("newMessage", messagesContent);
        }

        /// <summary>
        /// Send value to tracking the data.
        /// </summary>
        /// <param name="track">track value</param>
        public async Task TrackingData(bool track)
        {
            await Clients.All.SendAsync("newOrder", track);
        }

        #endregion

        #region Utitilities

        /// <summary>
        ///  register client to the database.
        /// </summary>
        /// <param name="registerToClientDto">client's request</param>
        /// <param name="foundRoom">Room's request</param>
        /// <returns></returns>
        private async Task RegisterClient(ClientToRegisterRequest registerToClientDto, Room foundRoom)
        {
            var roomsToRegister = new List<ClientAndRoom>();

            Client clientForRegister = _mapper.Map<Client>(registerToClientDto);
            string currentClientConnectionId = Context.ConnectionId;

            if (foundRoom is null)
            {
                Room roomToCreate = new Room
                {
                    Name = registerToClientDto.RoomName,
                    OrderId = registerToClientDto.OrderId,
                    AppType = registerToClientDto.ApplicationType,
                    CreatedAt = DateTime.Now
                };

                roomsToRegister.Add(new ClientAndRoom
                {
                    Client = clientForRegister,
                    Room = roomToCreate
                });

                clientForRegister.ConnectionId = currentClientConnectionId;
                clientForRegister.ClientAndRooms = roomsToRegister;
                await _clientService.CreateClient(clientForRegister);
            }
            else
            {
                clientForRegister.ConnectionId = currentClientConnectionId;
                roomsToRegister.Add(new ClientAndRoom
                {
                    Client = clientForRegister,
                    Room = foundRoom
                });
                clientForRegister.ClientAndRooms = roomsToRegister;
                await _clientService.CreateClient(clientForRegister);
            }
          
            await Groups.AddToGroupAsync(currentClientConnectionId, registerToClientDto.RoomName);
        }


        /// <summary>
        /// join client to the room.
        /// </summary>
        /// <param name="registeredClient">client's request</param>
        /// <param name="room">room's name</param>
        /// <param name="orderId">room's orderId</param>
        private async Task UserJoinToRoom(Client registeredClient, string room , int orderId)
        {
            var registerRooms = new List<ClientAndRoom>();

            string currentUserConnectionId = Context.ConnectionId;

            Room foundRoom = await _roomService.GetRoomByName(room);

            if (registeredClient.ConnectionId is null || 
                !registeredClient.ConnectionId.Equals(currentUserConnectionId))
            {
                registeredClient.ConnectionId = currentUserConnectionId;
                await Groups.RemoveFromGroupAsync(registeredClient.ConnectionId, room);
                await Groups.AddToGroupAsync(currentUserConnectionId, room);
            }

            if(foundRoom != null)
            {
                if (!await _roomService.RoomContainClient(registeredClient.Id, foundRoom.Id))
                {
                    registeredClient.ClientAndRooms.Add(new ClientAndRoom
                    {
                        Room = foundRoom,
                        Client = registeredClient
                    });
                }
            }
            else
            {
                Room registerRoom = new Room
                {
                    AppType = registeredClient.AppType,
                    CreatedAt = DateTime.Now,
                    Name = room,
                    OrderId = orderId
                };

                await _roomService.CreateRoom(registerRoom);

                registeredClient.ClientAndRooms.Add(new ClientAndRoom
                {
                    Room = registerRoom,
                    Client = registeredClient
                });

            }
                     
           await _clientService.UpdateClient(registeredClient);
        }

        #endregion

    }
}
