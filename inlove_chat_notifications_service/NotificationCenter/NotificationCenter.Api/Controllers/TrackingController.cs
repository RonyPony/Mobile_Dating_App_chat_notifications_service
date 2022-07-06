using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fcm = FirebaseAdmin.Messaging;
using System;

namespace NotificationCenter.Api.Controllers
{
    /// <summary>
    /// Represent the tracking controller. This controller is intended to send push notifications about the route updates of an order.
    /// </summary>
    [Route("tracking")]
    [ApiController]
    public sealed class TrackingController : ControllerBase
    {
        private readonly NotificationCenterContext _context;
        private readonly fcm.FirebaseMessaging _firebaseMessaging;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingController"/> class.
        /// </summary>
        /// <param name="context">An instance of the <see cref="NotificationCenterContext"/> class.</param>
        /// <param name="firebaseMessaging">>An instance of the <see cref="fcm.FirebaseMessaging"/> class.</param>
        public TrackingController(NotificationCenterContext context, fcm.FirebaseMessaging firebaseMessaging)
        {
            _context = context;
            _firebaseMessaging = firebaseMessaging;
        }

        /// <summary>
        /// Sends a data message to the user with updated information about the status of an order.
        /// </summary>
        /// <param name="orderId">The Id of the order to update.</param>
        /// <param name="userId">The Id of the user that will receive the notification.</param>
        /// <param name="updateRequest">The data that will be sent to the user.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Updatemarker([FromQuery] string orderId, [FromQuery] string userId, [FromBody] OrderUpdateRequest updateRequest)
        {
            TokenUser user = await _context.TokenUsers
                .Include(user => user.Devices)
                .FirstOrDefaultAsync(user => user.UserId == userId);

            if (user == null) return BadRequest();

            if (user.Devices == null || user.Devices.Count() <= 0) return BadRequest();

            bool showPhone = false;

            var contactEntry = _context.OrderContactStatuses.FirstOrDefault(e => e.OrderId == orderId);

            if (contactEntry != null)
            {
                showPhone = true;
                if (updateRequest.OrderStatus == OrderStatus.Delivered || updateRequest.OrderStatus == OrderStatus.DeclinedByStore)
                    _context.OrderContactStatuses.Remove(contactEntry);

            }
            else if (contactEntry == null && updateRequest.Driver != null && updateRequest.Driver.CanBeContactedByCustomer)
            {
                showPhone = true;
                _context.OrderContactStatuses.Add(new OrderContactStatus { OrderId = orderId });
            }

            await _context.SaveChangesAsync();

            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "notificationType", "orderStatusUpdate" },
                { "orderId", orderId },
                { "latitude", updateRequest.Coordinates?.Latitude.ToString() },
                { "longitude", updateRequest.Coordinates?.Longitude.ToString() },
                { "shipmentStatus", ((int)updateRequest.ShipmentStatus).ToString() },
                { "orderStatus", ((int)updateRequest.OrderStatus).ToString() },
                { "polyline", updateRequest.Polyline },
                { "driverId", updateRequest.Driver?.Id.ToString() },
                { "driverName", updateRequest.Driver?.DriverName },
                { "driverImageUrl", updateRequest.Driver?.DriverImageUrl },
                { "driverPhoneNumber", updateRequest.Driver?.DriverPhoneNumber },
                { "canContactDriver" , showPhone.ToString() },
                {"estimatedTime", updateRequest.EstimatedTime },
                {"destinationPlace", updateRequest.DestinationPlace },
            };

            var userTokens = user.Devices.Select(d => d.FcmToken).ToList();

            var message = new fcm.MulticastMessage
            {
                Data = dict,
                Tokens = user.Devices.Select(de => de.FcmToken).ToList(),
            };

            fcm.BatchResponse fcmresponse = await _firebaseMessaging.SendMulticastAsync(message);

            return Ok();

        }
    }
}

