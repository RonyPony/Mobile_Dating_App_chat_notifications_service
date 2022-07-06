using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Api.Controllers
{
    /// <summary>
    /// Represents the Room controller
    /// </summary>
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        #region Fields
        private readonly IRoomService _roomService;
        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of <see cref="RoomController"/>
        /// </summary>
        /// <param name="roomService">An implementation of <see cref="IRoomService"/>.</param>    
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult GetRooms()
        {
            try
            {
                IList<Room> rooms = _roomService.GetRooms();

                return Ok(rooms);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{roomName}")]
        public async Task<IActionResult> GetRoomByName(string roomName)
        {
            try
            {
                Room room = await _roomService.GetRoomByName(roomName);
                return Ok(room);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{orderId}/order")]
        public async Task<IActionResult> GetRoomByOrderId(int orderId)
        {
            try
            {
                Room room = await _roomService.GetRoomByOrderId(orderId);
                return Ok(room);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
                  
        }

        #endregion
    }
}
