using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Managers;
using NotificationCenter.Core.Models;
using System.Threading.Tasks;

namespace NotificationCenter.Api.Controllers
{
    /// <summary>
    /// Represents the tokens controller
    /// </summary>
    [Route("tokens")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly TokenManager _tokenManager;
        private readonly ILogger<TokensController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokensController"/> class.
        /// </summary>
        /// <param name="tokenManager">An instance of <see cref="TokenManager"/>.</param>
        /// <param name="logger">An instance of a logger.</param>
        public TokensController(TokenManager tokenManager, ILogger<TokensController> logger)
        {
            _tokenManager = tokenManager;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new FCM Token
        /// </summary>
        /// <param name="userTokenRequest">A <see cref="UserTokenRequest"/> object</param>
        /// <returns>A 200 status code if registered successfully.</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> EstablishUserToken([FromBody] UserTokenRequest userTokenRequest)
        {
            IOperationResult<Device> operationResult = await _tokenManager.EstablishUserToken(userTokenRequest);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(operationResult.Data);
        }

        /// <summary>
        /// Removes a token from records
        /// </summary>
        /// <param name="deviceIdentifier">Represents the value that identifies the device globally</param>
        /// <param name="appName">Represents the app to which this identifier is asociated</param>
        /// <returns>A 200 status code with the deleted device, if sueccessful.</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Device), 200)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> RemoveDeviceToken([FromQuery] string deviceIdentifier, [FromQuery] string appName)
        {
            IOperationResult<Device> operationResult = await _tokenManager.RemoveDeviceByIdentifier(deviceIdentifier, appName);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(operationResult.Data);
        }
    }
}
