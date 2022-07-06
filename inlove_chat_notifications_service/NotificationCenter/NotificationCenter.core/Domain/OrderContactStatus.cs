namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents an indication that an order has an active contact status.
    /// </summary>
    public sealed class OrderContactStatus
    {
        /// <summary>
        /// Represents the ID of this entry
        /// </summary>]
        public int Id { get; set; }

        /// <summary>
        /// The Id of the order where the driver authorized contact
        /// </summary>
        public string OrderId { get; set; }
    }
}
