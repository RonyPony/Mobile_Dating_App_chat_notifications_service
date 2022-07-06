using NotificationCenter.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents the client who will interact in the messages.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// Get or set id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Get or set client username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Get or set client connectionId
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// Get or set app type.
        /// </summary>
        public AppType AppType { get; set; }

        /// <summary>
        /// Get or set  rooms
        /// </summary>
        public List<ClientAndRoom> ClientAndRooms { get; set; }

        /// <summary>
        /// Get or set connection status.
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Get or set client register date.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
