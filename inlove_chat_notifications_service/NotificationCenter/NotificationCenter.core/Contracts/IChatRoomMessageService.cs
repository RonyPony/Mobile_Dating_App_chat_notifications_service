using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents a contract to chat message room for the app.
    /// </summary>
    public interface IChatRoomMessageService
    {
        /// <summary>
        /// Retrieves a list of messages of the chat by spefic room. 
        /// </summary>
        /// <param name="roomId">Room's id</param>
        /// <returns>a list of chat message </returns>
        IList<ChatRoomMessages> GetMessageOfRoomByRoomId(int roomId);

        /// <summary>
        ///  Insert a chat room message.
        /// </summary>
        /// <param name="roomMessages">ChatRoomMessages's request</param>
        /// <returns></returns>
        Task InsertChatRoomMessage(ChatRoomMessages roomMessages);
    }
}
