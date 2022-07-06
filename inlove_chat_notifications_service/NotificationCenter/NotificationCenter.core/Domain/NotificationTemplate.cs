using System;
using System.Collections.Generic;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents a template for notifications to be sent.
    /// </summary>
    public sealed class NotificationTemplate
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
        /// Represents whether a high priority notification should be sent
        /// </summary>
        public bool HighPriority { get; set; } = false;

        /// <summary>
        /// The type of template this is. This determines in wich situations the template should be used.
        /// </summary>
        public TemplateType TemplateType { get; set; }

        /// <summary>
        /// The log of notifications sent using this template.
        /// </summary>
        public IEnumerable<NotificationTemplateSendHistory> History { get; set; }

        /// <summary>
        /// Whether this template is currently in use or not.
        /// </summary>
        public bool IsActive { get; set; }
    }   
}
