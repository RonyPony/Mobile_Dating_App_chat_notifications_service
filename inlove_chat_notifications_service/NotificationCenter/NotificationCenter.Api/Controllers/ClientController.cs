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
    /// Represents the client controller
    /// </summary>
    [Route("api/clients")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        #region Fields
        private readonly IClientService _clientService;
        #endregion

        #region Ctor

        /// <summary>
        /// Creates an instance of <see cref="ClientController"/>
        /// </summary>
        /// <param name="clientService">An implementation of <see cref="IClientService"/>.</param>
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public  IActionResult GetClients()
        {
            try
            {
                IList<Client> clients = _clientService.GetClients();
                return Ok(clients);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetClientByUsername(string username)
        {
            try
            {
                Client client = await _clientService.GetClientByUsername(username);

                return Ok(client);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion
    }
}
