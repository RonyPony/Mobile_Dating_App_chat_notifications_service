using System;
using System.Collections.Generic;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a request to create a new <see cref="TemplateType"/>.
    /// </summary>
    public class TemplateTypeRequest
    {
        /// <summary>
        /// The public unique Id of this template type. Intended to be a human readable string.
        /// </summary>
        public String ReadableId { get; set; }

        /// <summary>
        /// The name of this template type.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The description of this template type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// The paramters that this type of notification should use
        /// </summary>
        public List<NotificationTemplateParamRequest> TemplateParams { get; set; }
    }
}
