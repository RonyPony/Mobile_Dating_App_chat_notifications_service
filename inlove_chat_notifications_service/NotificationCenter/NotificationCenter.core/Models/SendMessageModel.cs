using NotificationCenter.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a send message model.
    /// </summary>
    public sealed class SendMessageModel
    {
        /// <summary>
        /// Get or set order's id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Get or set user's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Get or set username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Get or set user's message
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// Get or set message date.
        /// </summary>
        public string SentTime { get; set; }
    }
}
