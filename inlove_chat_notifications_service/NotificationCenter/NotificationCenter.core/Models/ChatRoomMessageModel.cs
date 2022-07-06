using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents chat room message model.
    /// </summary>
    public sealed class ChatRoomMessageModel
    {
        /// <summary>
        /// Get or set client's username
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Get or set client's id.
        /// </summary>
        [JsonProperty("clientId")]
        public int ClientId { get; set; }

        /// <summary>
        /// Get or set Room's id
        /// </summary>
        [JsonProperty("roomId")]
        public int RoomId { get; set; }

        /// <summary>
        /// Get or set room's content
        /// </summary>
        [JsonProperty("messageContent")]
        public string MessageContent { get; set; }

        /// <summary>
        /// Get or set register date.
        /// </summary>
        [JsonProperty("createdOnUtc")]
        public DateTime CreatedOnUtc { get; set; }
    }
}
