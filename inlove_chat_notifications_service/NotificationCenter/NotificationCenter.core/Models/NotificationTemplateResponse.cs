using System;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a response of a <see cref="NotificationTemplate"/>.
    /// </summary>
    public class NotificationTemplateResponse : NotificationTemplateBase
    {
        /// <summary>
        /// Represents the ID of the notification template
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the <see cref="TemplateType"/> of this template.
        /// </summary>
        public TemplateTypeResponse TemplateType { get; set; }
    }
}
