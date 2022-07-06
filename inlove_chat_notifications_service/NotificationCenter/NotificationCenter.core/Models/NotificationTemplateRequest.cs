using System.ComponentModel.DataAnnotations;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents the base for <see cref="NotificationTemplateRequest"/> and <see cref="NotificationTemplateResponse"/>.
    /// </summary>
    public class NotificationTemplateBase
    {
        /// <summary>
        /// The title sent with the notification
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// The basic body of the notification
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Represents whether a high priority notification should be sent
        /// </summary>
        public bool HighPriority { get; set; } = false;
    }

    /// <summary>
    /// Represenst a request to add a new <see cref="NotificationTemplate"/>
    /// </summary>
    public class NotificationTemplateRequest : NotificationTemplateBase
    {
        /// <summary>
        /// The Id of the <see cref="TemplateType"/> of this template.
        /// </summary>
        public string TemplateTypeId { get; set; }
    }
}
