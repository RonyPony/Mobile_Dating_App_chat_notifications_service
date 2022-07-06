using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Services
{
    /// <summary>
    /// Represents a room service implementation for the app.
    /// </summary>
    public class RoomService : IRoomService
    {
        #region Fields

        private readonly NotificationCenterContext _notificationCenterContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of <see cref="CustomerController"/>
        /// </summary>
        /// <param name="notificationCenterContext">An implementation of <see cref="NotificationCenterContext"/>.</param>
        public RoomService(NotificationCenterContext notificationCenterContext)
        {
            _notificationCenterContext = notificationCenterContext;
        }

        #endregion

        #region Methods

        ///<inheritdoc/>
        public async Task CreateRoom(Room room)
        {
            if (room is null)
                throw new ArgumentException("InvalidRoomRequest");

            room.CreatedAt = DateTime.Now;
            _notificationCenterContext.Rooms.Add(room);
            await _notificationCenterContext.SaveChangesAsync();
        }

        ///<inheritdoc/>
        public async Task<Room> GetRoomByName(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
                throw new ArgumentException("InvalidRoom");

            return await _notificationCenterContext.Rooms
                .FirstOrDefaultAsync(room => room.Name.Equals(roomName));
        }

        ///<inheritdoc/>
        public async Task<Room> GetRoomByOrderId(int orderId)
        {
            if (orderId == 0)
                throw new ArgumentException("InvalidOrder");

            return await _notificationCenterContext.Rooms.Include(room => room.ClientAndRooms)
                .FirstOrDefaultAsync(room => room.OrderId == orderId);
        }

        ///<inheritdoc/>
        public IList<Room> GetRooms()
        {
            return _notificationCenterContext
                 .Rooms
                 .Include(room => room.ClientAndRooms).ToList();
        }

        ///<inheritdoc/>
        public async Task<bool> RoomContainClient(int clientId , int roomId)
        {
            if (clientId == 0)
                throw new ArgumentException("InvalidClientId");

            if (roomId == 0)
                throw new ArgumentException("InvalidRoomId");

            return await _notificationCenterContext
                .ClientAndRoom.AnyAsync(rc => rc.ClientId == clientId && rc.RoomId == roomId);
        }

        ///<inheritdoc/>
        public async Task UpdateRoom(Room room)
        {
            if (room is null)
                throw new ArgumentException("InvalidRoomRequest");

            _notificationCenterContext.Rooms.Update(room);
            await _notificationCenterContext.SaveChangesAsync();
        }

        #endregion
    }
}
