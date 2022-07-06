using System.ComponentModel.DataAnnotations;

namespace NotificationCenter.Core.Domain
{
    /// <summary>
    /// Represents the relation between client and room.
    /// </summary>
    public class ClientAndRoom
    {
        /// <summary>
        /// Get or set id.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Get or set client id.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Get or set client.
        /// </summary>
        public Client Client { get; set; }

        /// <summary>
        /// Get or set room id.
        /// </summary>
        public int RoomId { get; set; }

        /// <summary>
        /// Get or set Room.
        /// </summary>
        public Room Room { get; set; }
    }
}
