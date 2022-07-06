using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationCenter.Core.Domain
{
    public class ChatRoomMessages
    {
        /// <summary>
        ///Get or set id. 
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Get or set Client's id.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Get or set Room's id
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Get or set room's content
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// Get or set register date.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
