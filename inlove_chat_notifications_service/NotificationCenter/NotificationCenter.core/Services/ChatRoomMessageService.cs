using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Services
{
    /// <summary>
    /// Represents the chat room message service implementations.
    /// </summary>
    public class ChatRoomMessageService : IChatRoomMessageService
    {
        private readonly NotificationCenterContext _notificationCenterContext;

        public ChatRoomMessageService(NotificationCenterContext notificationCenterContext)
        {
            _notificationCenterContext = notificationCenterContext;
        }

        public IList<ChatRoomMessages> GetMessageOfRoomByRoomId(int roomId)
        {
            if (roomId == 0)
                throw new ArgumentException("InvalidRoomId");

            return _notificationCenterContext.RoomMessages
                .Where(room => room.RoomId == roomId)
                .ToList();
        }

        public async Task InsertChatRoomMessage(ChatRoomMessages roomMessages)
        {
            if (roomMessages is null)
                throw new ArgumentException("InvalidRoomMessageRequest");

            _notificationCenterContext.RoomMessages.Add(roomMessages);
            await _notificationCenterContext.SaveChangesAsync();
        }
    }
}
