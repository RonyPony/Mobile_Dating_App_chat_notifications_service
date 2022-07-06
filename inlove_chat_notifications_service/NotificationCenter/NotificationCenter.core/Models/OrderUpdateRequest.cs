namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents a request to update an order status.
    /// </summary>
    public class OrderUpdateRequest
    {
        /// <summary>
        /// The new coordinates to send to the user.
        /// </summary>
        public Coordinates Coordinates { get; set; }

        public ShippingStatus ShipmentStatus { get; set; }

        /// <summary>
        /// The new order status to send to the user.
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// The new polyline to send to the user.
        /// </summary>
        public string Polyline { get; set; }

        /// <summary>
        /// The new Driver information to send to the user.
        /// </summary>
        public Driver Driver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EstimatedTime { get; set; }

        public string DestinationPlace { get; set; }
    }

    /// <summary>
    /// Represents the driver assigned to an order.
    /// </summary>
    public class Driver
    {
        /// <summary>
        /// The Id of the driver.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the driver.
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// The Url of the photo of the driver.
        /// </summary>
        public string DriverImageUrl { get; set; }

        /// <summary>
        /// The phone number of the driver.
        /// </summary>
        public string DriverPhoneNumber { get; set; }

        /// <summary>
        /// Indicates that the driver has stablished comunication with the client, so the client can contact the driver back.
        /// </summary>
        public bool CanBeContactedByCustomer { get; set; }
    }

    /// <summary>
    /// Represents a point on Earth.
    /// </summary>
    public class Coordinates
    {
        /// <summary>
        /// The latitude of this point.
        /// </summary>
        public decimal Latitude { get; set; }

        /// <summary>
        /// The longitude of this point
        /// </summary>
        public decimal Longitude { get; set; }
    }

    /// <summary>
    /// The posible statuses of an order, equivalent to nopCommerce statuses.
    /// </summary>
    public enum OrderStatus
    {
        AwaitingForMessenger = 1,
        AssignedToMessenger = 2,
        OrderPreparationCompleted = 3,
        DeliveryInProgress = 4,
        Delivered = 5,
        DeclinedByStore = 6
    }

    public enum ShippingStatus
    {
        /// <summary>
        /// Shipping not required
        /// </summary>
        ShippingNotRequired = 10,
        /// <summary>
        /// Not yet shipped
        /// </summary>
        NotYetShipped = 20,
        /// <summary>
        /// Partially shipped
        /// </summary>
        PartiallyShipped = 25,
        /// <summary>
        /// Shipped
        /// </summary>
        Shipped = 30,
        /// <summary>
        /// Delivered
        /// </summary>
        Delivered = 40
    }
}
