using NotificationCenter.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents a log of a push notification sent using a <see cref="NotificationTemplate"/>
    /// </summary>
    public sealed class NotificationTemplateSendHistory
    {
        /// <summary>
        /// Represents the ID of the notification
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Represents the ID of the user receiving the TOKEN
        /// </summary>
        public Guid TokenUserId { get; set; }

        /// <summary>
        /// Represents the user receiving the TOKEN
        /// </summary>
        public TokenUser TokenUser { get; set; }

        /// <summary>
        /// The Id of the <see cref="NotificationTemplate"/> sent.
        /// </summary>
        public Guid NotificationTemplateId { get; set; }

        /// <summary>
        /// The <see cref="NotificationTemplate"/> sent.
        /// </summary>
        public NotificationTemplate NotificationTemplate { get; set; }

        /// <summary>
        /// The date when the notification was sent.
        /// </summary>
        public DateTime DateSent { get; set; }

        /// <summary>
        /// The date when the notification was read.
        /// </summary>
        public DateTime? DateRead { get; set; }

        /// <summary>
        /// Indicates how this notification was read.
        /// </summary>
        public ReadMethod ReadMethod { get; set; } = ReadMethod.None;

        /// <summary>
        /// Returns true if the notification was read, false otherwise.
        /// </summary>
        [NotMapped]
        public bool IsRead { get => DateRead != null; }

        /// <summary>
        /// If the user has seen this notification and decided to ignore it. 
        /// </summary>
        public bool Ignored { get; set; } = false;

        /// <summary>
        /// The total number of devices that should have received the push notification.
        /// </summary>
        public int DevicesCount { get; set; }

        /// <summary>
        /// The number of devices that successfully received the notification.
        /// </summary>
        public int DeveicesSentTo { get; set; }

        /// <summary>
        /// The number of devices that failed to receive the notification.
        /// </summary>
        public int FailedDevices { get; set; }

        /// <summary>
        /// The payload sent with the notification.
        /// </summary>
        public string Payload { get; set; }
    }
}
