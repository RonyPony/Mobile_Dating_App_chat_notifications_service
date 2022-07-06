using System;
using NotificationCenter.Core.Enums;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents a Registered Device to send notifications
    /// </summary>
    public sealed class Device
    {
        /// <summary>
        /// Represents the ID of the device
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Represents the type of the device
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// Represents the device's token
        /// </summary>
        public string FcmToken { get; set; }

        /// <summary>
        /// Represents the APNS Token for iOS devices
        /// </summary>
        public string ApnsToken { get; set; }

        /// <summary>
        /// Represents the global identification number for the physical device
        /// </summary>
        public string DeviceIdentifier { get; set; }

        /// <summary>
        /// The name of the application to which this token registration belongs.
        /// </summary>
        public App App { get; set; }

        /// <summary>
        /// Represents the ID of the app
        /// </summary>
        public Guid AppId { get; set; }

        /// <summary>
        /// Represents the ID of the owner of the token
        /// </summary>
        public Guid TokenUserId { get; set; }

        /// <summary>
        /// Represents the token user
        /// </summary>
        public TokenUser TokenUser { get; set; }
    }
}
