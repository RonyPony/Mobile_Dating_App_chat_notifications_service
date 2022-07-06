using System;
using System.Collections.Generic;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents the reference to the user on the implementing service
    /// </summary>
    public sealed class TokenUser
    {
        /// <summary>
        /// Represents the internal ID representing the user
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Represents the ID of the User in the original system
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Represents the name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents the lastName of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents the list of devices
        /// </summary>
        public IEnumerable<Device> Devices { get; set; }

        /// <summary>
        /// Represents the sent notifications
        /// </summary>
        public IEnumerable<NotificationTemplateSendHistory> NotificationTemplate { get; set; }
    }
}
