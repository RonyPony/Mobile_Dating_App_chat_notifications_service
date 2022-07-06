using System.ComponentModel.DataAnnotations;
using NotificationCenter.Core.Enums;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a request to add a new device token
    /// </summary>
    public sealed class UserTokenRequest
    {
        /// <summary>
        /// Represents the FCM or APNs token associated to the device
        /// </summary>
        [Required]
        public string DeviceToken { get; set; }

        /// <summary>
        /// Represents the APNS Token for iOS devices
        /// </summary>
        public string ApnsToken { get; set; }

        /// <summary>
        /// Represents the global device identifier
        /// </summary>
        [Required]
        public string DeviceIdentifier { get; set; }

        /// <summary>
        /// Represents the app to which this token is asociated
        /// </summary>
        [Required]
        public string AppName { get; set; }

        /// <summary>
        /// Represents the type of device
        /// </summary>
        [Required]
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Represents the User to which the token belongs to in the original app
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Represents the name of the user
        /// </summary>
        [MinLength(3)]
        public string Name { get; set; }

        /// <summary>
        /// Represents the last name of the user
        /// </summary>
        [MinLength(3)]
        public string LastName { get; set; }
    }
}