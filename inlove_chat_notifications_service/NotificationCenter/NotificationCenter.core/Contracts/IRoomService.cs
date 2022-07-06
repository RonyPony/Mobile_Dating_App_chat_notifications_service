using NotificationCenter.Core.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents a contract for the service of this app.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Retrives a list of rooms .
        /// </summary>
        /// <returns>list of registered room</returns>
        IList<Room> GetRooms();

        /// <summary>
        /// Retrives a specific room by name.
        /// </summary>
        /// <param name="roomName">the room name.</param>
        /// <returns>specific room</returns>
        Task<Room> GetRoomByName(string roomName);

        /// <summary>
        /// Retrives a specific room by order id.
        /// </summary>
        /// <param name="orderId">client's order id.</param>
        /// <returns>specific room</returns>
        Task<Room> GetRoomByOrderId(int orderId);

        /// <summary>
        /// Register a room.
        /// </summary>
        /// <param name="room">room request.</param>
        Task CreateRoom(Room room);

        /// <summary>
        /// Update the room.
        /// </summary>
        /// <param name="room">room request</param>
        Task UpdateRoom(Room room);

        /// <summary>
        /// Evaluate if room contain the client.
        /// </summary>
        /// <param name="clientId">Client's id</param>
        /// <param name="roomId">Room's id</param>
        /// <returns></returns>
        Task<bool> RoomContainClient(int clientId , int roomId);
    }
}
