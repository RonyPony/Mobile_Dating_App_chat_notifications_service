using System.ComponentModel.DataAnnotations;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a request to add a new <see cref="NotificationDataDetails"/>.
    /// </summary>
    public class NotificationTemplateParamRequest
    {
        /// <summary>
        /// Represents the Key of the data attribute
        /// </summary>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// The description of this parameter, i.e. an explanation of the data expected for this param.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// The data type of this parameter, i.e. if its suposed to be text, a number, a date, etc.
        /// </summary>
        [Required]
        public ExpectedDataType DataType { get; set; }
    }
}