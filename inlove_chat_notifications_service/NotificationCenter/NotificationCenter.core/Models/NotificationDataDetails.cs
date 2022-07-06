using NotificationCenter.Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents the Data details of a notification
    /// </summary>
    public class NotificationDataDetails
    {
        /// <summary>
        /// The Id of this object.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// The tempate to which this <see cref="NotificationDataDetails"/> corresponds.
        /// </summary>
        public TemplateType Template { get; set; }

        /// <summary>
        /// Represents the Key of the data attribute
        /// </summary>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// Represents teh value of the data attribute
        /// </summary>
        [NotMapped]
        public string Value { get; set; }

        /// <summary>
        /// The description of this <see cref="NotificationDataDetails"/>
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The data type expected for the value of this <see cref="NotificationDataDetails"/>
        /// </summary>
        public ExpectedDataType DataType { get; set; }
    }

    public enum ExpectedDataType
    {
        Text,
        Number,
        Decimal,
        Date,
    }
}