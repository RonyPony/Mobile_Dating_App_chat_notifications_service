using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Enums;
using NotificationCenter.Core.Managers;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Controllers.Controllers
{
    /// <summary>
    /// Represents the notification controller
    /// </summary>
    [Route("notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly TokenManager _tokenManager;
        private readonly TemplateTypeManager _templateTypeManager;
        private readonly NotificationTemplateManager _notificationTemplateManager;
        private readonly IMapper _mapper;
        private readonly Random Random = new Random();
        private readonly ILogger<NotificationsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationsController"/> class.
        /// </summary>
        /// <param name="notificationTemplateManager">An instance of <see cref="NotificationTemplateManager"/>.</param>
        /// <param name="templateTypeManager">An instance of <see cref="TemplateTypeManager"/>.</param>
        /// <param name="tokenManager">An instance of <see cref="TokenManager"/>.</param>
        /// <param name="mapper">An implementation of the <see cref="IMapper"/> interface.</param>
        /// <param name="logger">An instance of a logger.</param>
        public NotificationsController(NotificationTemplateManager notificationTemplateManager, TemplateTypeManager templateTypeManager, TokenManager tokenManager, IMapper mapper, ILogger<NotificationsController> logger)
        {
            _notificationTemplateManager = notificationTemplateManager;
            _templateTypeManager = templateTypeManager;
            _tokenManager = tokenManager;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Sends a notification from a template to all devices of the specified user.
        /// </summary>
        /// <param name="templateId">The Id of the template to send</param>
        /// <param name="userId">The Id of the user that will receive the notification</param>
        /// <param name="appName">The package name of the app that should receive this notification.</param>
        /// <param name="data">The required parameters for this speicific template. These will be used to customize the template and to be sent in the notification payload</param>
        /// <returns>A 200 status code with an object containing the Id of the resulting <see cref="NotificationTemplateSendHistory"/>.</returns>
        [HttpPost]
        [Route("status/send-template/{templateId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> SendNotificationFromTemplate([FromRoute] string templateId, [FromQuery] string userId, [FromQuery] string appName, [FromBody] IDictionary<string, dynamic> data)
        {
            _ = Guid.TryParse(templateId, out Guid parsedTemplateId);

            IOperationResult<NotificationTemplateSendHistory> operationResult = await _notificationTemplateManager.SendNotification(parsedTemplateId, userId, appName, data, GetReadUrl(Request));

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(new { logId = operationResult.Data.Id });
        }

        /// <summary>
        /// Sends a notification from a tempate type to all devices of the specified user.
        /// </summary>
        /// <param name="templateType">The Id of the template type which will be used to find a template to send</param>
        /// <param name="userId">The Id of the user that will receive the notification</param>
        /// <param name="appName">The package name of the app that should receive this notification.</param>
        /// <param name="data">The required parameters for this speicific template. These will be used to customize the template and to be sent in the notification payload</param>
        /// <returns>A 200 status code with an object containing the Id of the resulting <see cref="NotificationTemplateSendHistory"/>.</returns>
        [HttpPost]
        [Route("status/send-type/{templateType}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> SendNotificationFromTemplateType([FromRoute] string templateType, [FromQuery] string userId, [FromQuery] string appName, [FromBody] IDictionary<string, dynamic> data)
        {
            IOperationResult<TemplateType> type = await _templateTypeManager.GetTemplateType(templateType);

            if (!type.Success)
                return BadRequest(type);

            if (type.Data.Templates == null || type.Data.Templates?.Count <= 0)
                return BadRequest();

            NotificationTemplate template = type.Data.Templates[Random.Next(0, type.Data.Templates.Count)];

            IOperationResult<NotificationTemplateSendHistory> operationResult = await _notificationTemplateManager.SendNotification(template.Id, userId, appName, data, GetReadUrl(Request));

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(new { logId = operationResult.Data.Id });
        }

        /// <summary>
        /// Marks a sent template notification as read
        /// </summary>
        /// <param name="id">The ID of the notification template send log</param>
        /// <param name="method">Indicates how the notification was read</param>
        /// <returns>A 204 status code if the operation was successful.</returns>
        [HttpPatch]
        [Route("status/read")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> MarkNotificationAsRead([FromQuery] string id, [Required] ReadMethod method)
        {
            _ = Guid.TryParse(id, out Guid parsedId);

            IOperationResult<NotificationTemplateSendHistory> operationResult = await _notificationTemplateManager.MarkNotificationAsRead(parsedId, method);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return NoContent();
        }

        /// <summary>
        /// Marks many sent notifications as read
        /// </summary>
        /// <param name="notificationsId">A list of the notifications to be marked as read.</param>
        /// <returns>A 204 status code if the operation was successful.</returns>
        [HttpPatch]
        [Route("status/read-many")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> MarkManyNotificationsAsRead([FromBody] List<string> notificationsId)
        {
            List<Guid> parsedNotificationsIds = notificationsId.Select(id => Guid.Parse(id)).ToList();

            IOperationResult<bool> operationResult = await _notificationTemplateManager.MarkManyNotificationsAsRead(parsedNotificationsIds);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return NoContent();
        }

        /// <summary>
        /// Marks many sent notifications as ignored
        /// </summary>
        /// <param name="notificationsId">A list of the notifications to be marked as ignored.</param>
        /// <returns>A 204 status code if the operation was successful.</returns>
        [HttpPatch]
        [Route("status/ignore-many")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> MarkManyNotificationsAsIgnored([FromBody] List<string> notificationsId)
        {
            List<Guid> parsedNotificationsIds = notificationsId.Select(id => Guid.Parse(id)).ToList();

            IOperationResult<bool> operationResult = await _notificationTemplateManager.MarkManyNotificationsAsIgnored(parsedNotificationsIds);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return NoContent();
        }

        /// <summary>
        /// Marks a sent template notification as ignored
        /// </summary>
        /// <param name="id">The ID of the notification template send log</param>
        /// <returns>A 204 status code if the operation was successful.</returns>
        [HttpPatch]
        [Route("status/ignore")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> MarkNotificationAsIgnored([FromQuery] string id)
        {
            _ = Guid.TryParse(id, out Guid parsedId);

            IOperationResult<NotificationTemplateSendHistory> operationResult = await _notificationTemplateManager.MarkNotificationAsIgnored(parsedId);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return NoContent();
        }

        /// <summary>
        /// Gets all notifications for a given user
        /// </summary>
        /// <param name="userId">The Id of the user who's notification will be fetched</param>
        /// <param name="count">The number of items to get per page</param>
        /// <param name="page">The page to get</param>
        /// <returns>A list of the notifications of the specified user.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> GetAllNotifications([FromQuery, Required] string userId, int count = 10, int page = 1)
        {
            IOperationResult<PaginatedList<Notification>> operationResult = await _notificationTemplateManager.GetSentNotificationsByUser(userId, count, page);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            operationResult.Data.Items.ForEach(item =>
            {
                item.ReadUrl = GetReadUrl(Request) + item.Id.ToString();
                item.IgnoreUrl = GetIgnoreUrl(Request) + item.Id.ToString();
            });

            return Ok(operationResult);
        }

        #region Private methods
        private string GetReadUrl(HttpRequest request)
        {
            UriBuilder uri = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? -1, Url.Action(nameof(MarkNotificationAsRead)).ToString());

            return $"{uri.Uri}?id=";
        }

        private string GetIgnoreUrl(HttpRequest request)
        {
            UriBuilder uri = new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port ?? -1, Url.Action(nameof(MarkNotificationAsIgnored)).ToString());

            return $"{uri.Uri}?id=";
        }
        #endregion
    }
}