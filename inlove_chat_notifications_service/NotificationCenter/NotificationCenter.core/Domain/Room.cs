using NotificationCenter.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents room created on app for the clients. 
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Get or set room's id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Get or set room's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set room's id.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Get or set room's apptype.
        /// </summary>
        public AppType AppType { get; set; }

        /// <summary>
        /// Get or set relation between client and room
        /// </summary>
        public List<ClientAndRoom> ClientAndRooms { get; set; }

        /// <summary>
        /// Get or set room's date.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
