using NotificationCenter.Core.Enums;
using System;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a notification already sent to the user
    /// </summary>
    public sealed class Notification
    {
        /// <summary>
        /// Represents the ID of the notification template
        /// </summary>
        public Guid Id { get; set; } = new Guid();

        /// <summary>
        /// The title sent with the notification
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The basic body of the notification
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The Id of the <see cref="NotificationTemplate"/> in which this notification  was based.
        /// </summary>
        public Guid NotificationTemplateId { get; set; }

        /// <summary>
        /// The date when the notification was sent.
        /// </summary>
        public DateTime DateSent { get; set; }

        /// <summary>
        /// The minutes since this notification was sent
        /// </summary>
        public int MinutesSinceSent { get; set; }

        /// <summary>
        /// Indicates if the notification was read.
        /// </summary>
        public bool Read => DateRead != null;

        /// <summary>
        /// The date when the notification was read.
        /// </summary>
        public DateTime? DateRead { get; set; }

        public bool Ignored { get; set; }
        public ReadMethod ReadMethod { get; set; }

        /// <summary>
        /// The url to call to mark this notification as read.
        /// </summary>
        public String ReadUrl { get; set; }

        /// <summary>
        /// The url to call to mark this notification as ignored.
        /// </summary>
        public String IgnoreUrl { get; set; }

        /// <summary>
        /// The payload sent with the notification.
        /// </summary>
        public string Payload { get; set; }
    }
}
