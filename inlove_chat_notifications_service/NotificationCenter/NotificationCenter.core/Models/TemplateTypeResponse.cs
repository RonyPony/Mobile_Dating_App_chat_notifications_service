using System.Collections.Generic;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a response of a <see cref="TemplateType"/>.
    /// </summary>
    public class TemplateTypeResponse
    {
        /// <summary>
        /// The public unique Id of this template type. Intended to be a human readable string.
        /// </summary>
        public string ReadableId { get; set; }

        /// <summary>
        /// The name of this template type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this template type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The parameters required by this template type.
        /// </summary>
        public List<NotificationTemplateParamResponse> TemplateParams { get; set; }

        /// <summary>
        /// The available templates of this template type.
        /// </summary>
        public List<NotificationTemplateResponse> Templates { get; set; }
    }

}
