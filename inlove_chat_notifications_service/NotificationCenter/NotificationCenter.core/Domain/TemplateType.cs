using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents a type of template to be used.
    /// </summary>
    public sealed class TemplateType
    {
        /// <summary>
        /// Represents the ID of the notification template
        /// </summary>
        public Guid Id { get; set; } = new Guid();

        /// <summary>
        /// Represents an unique identifier of the notification template that is assigned and is unique.
        /// </summary>
        public string ReadableId { get; set; }

        /// <summary>
        /// The name of this template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this template
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Represents the data sent with the notification
        /// </summary>
        public List<NotificationDataDetails> Data { get; set; }

        /// <summary>
        /// The list of <see cref="NotificationTemplate"/> of this template type.
        /// </summary>
        public List<NotificationTemplate> Templates { get; set; }

        /// <summary>
        /// Whether this template type is currently active or not.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
