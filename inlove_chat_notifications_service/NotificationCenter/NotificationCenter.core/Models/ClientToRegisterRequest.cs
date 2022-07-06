using NotificationCenter.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents client register Dto.
    /// </summary>
    public class ClientToRegisterRequest
    {
        /// <summary>
        /// Client username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// App type value.
        /// </summary>
        public AppType ApplicationType { get; set; }

        /// <summary>
        /// Room name.
        /// </summary>
        public string RoomName { get; set; }

        /// <summary>
        /// Order id assigned to client and room
        /// </summary>
        public int OrderId { get; set; }
    }
}
