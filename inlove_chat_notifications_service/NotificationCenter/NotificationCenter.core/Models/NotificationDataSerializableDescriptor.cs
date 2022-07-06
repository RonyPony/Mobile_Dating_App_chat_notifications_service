using System.Collections.Generic;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a class descriptor, used to serialize data
    /// </summary>
    public class NotificationDataSerializableDescriptor
    {
        /// <summary>
        /// The data containing the notification details
        /// </summary>
        public IEnumerable<NotificationDataDetails> data;
    }
}