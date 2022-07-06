using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Managers;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationCenter.Api.Controllers
{
    /// <summary>
    /// Represents the templates controller
    /// </summary>
    [Route("templates")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {
        private readonly TokenManager _tokenManager;
        private readonly TemplateTypeManager _templateTypeManager;
        private readonly NotificationTemplateManager _notificationTemplateManager;
        private readonly IMapper _mapper;
        private readonly Random Random = new Random();
        private readonly ILogger<TemplatesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplatesController"/> class.
        /// </summary>
        /// <param name="notificationTemplateManager">An instance of <see cref="NotificationTemplateManager"/>.</param>
        /// <param name="templateTypeManager">An instance of <see cref="TemplateTypeManager"/>.</param>
        /// <param name="tokenManager">An instance of <see cref="TokenManager"/>.</param>
        /// <param name="mapper">An implementation of the <see cref="IMapper"/> interface.</param>
        /// <param name="logger">An instance of a logger.</param>
        public TemplatesController(NotificationTemplateManager notificationTemplateManager, TemplateTypeManager templateTypeManager, TokenManager tokenManager, IMapper mapper, ILogger<TemplatesController> logger)
        {
            _notificationTemplateManager = notificationTemplateManager;
            _templateTypeManager = templateTypeManager;
            _tokenManager = tokenManager;
            _mapper = mapper;
            _logger = logger;
        }

        #region Templates management
        /// <summary>
        /// Gets the notification template with given ID
        /// </summary>
        /// <param name="Id">the ID to search</param>
        /// <returns>The template with the provided ID, if found</returns>
        [HttpGet]
        [Route("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> GetTemplate([FromRoute] string Id)
        {
            IOperationResult<NotificationTemplate> operationResult = await _notificationTemplateManager.GetNotificationTemplate(Guid.Parse(Id));

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(_mapper.Map<NotificationTemplateResponse>(operationResult.Data));
        }

        /// <summary>
        /// Creates a new notification template
        /// </summary>
        /// <param name="template">The template to create</param>
        /// <returns>the created notification</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(IOperationResult<bool>), 400)]
        public async Task<IActionResult> CreateNotificationTemplate([FromBody] NotificationTemplateRequest template)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IOperationResult<NotificationTemplate> operationResult = await _notificationTemplateManager.CreateNotificationTemplate(template);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return CreatedAtAction(nameof(GetTemplate), new { operationResult.Data.Id }, _mapper.Map<NotificationTemplateResponse>(operationResult.Data));
        }
        #endregion

        #region Template type management
        /// <summary>
        /// Gets a template type with the provided templateId
        /// </summary>
        /// <param name="typeId">The Id to search for.</param>
        /// <returns>A 200 status code with the found(if any) <see cref="TemplateType"/> if successful.</returns>
        [HttpGet, Route("types/{typeId}"), ProducesResponseType(200)]
        public async Task<IActionResult> GetTemplateType([FromRoute] string typeId)
        {
            IOperationResult<TemplateType> operationResult = await _templateTypeManager.GetTemplateType(typeId);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(_mapper.Map<TemplateTypeResponse>(operationResult.Data));
        }

        /// <summary>
        /// Gets all the template types.
        /// </summary>
        /// <returns>A list of the template types</returns>
        [HttpGet, Route("types"), ProducesResponseType(200)]
        public async Task<IActionResult> GetAllTemplateTypes()
        {
            IOperationResult<List<TemplateType>> operationResult = await _templateTypeManager.GetAllTemplateTypes();

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return Ok(_mapper.Map<List<TemplateType>, List<TemplateTypeResponse>>(operationResult.Data));
        }

        /// <summary>
        /// Creates a new template type
        /// </summary>
        /// <param name="request">The data of the <see cref="TemplateType"/> to create.</param>
        /// <returns>A 201 code with the created template type, or 400 if the template type could not be created</returns>
        [HttpPost, Route("types"), ProducesResponseType(201), ProducesResponseType(400)]
        public async Task<IActionResult> CreateTemplateType(TemplateTypeRequest request)
        {
            IOperationResult<TemplateType> operationResult = await _templateTypeManager.CreateTemplateType(_mapper.Map<TemplateType>(request));

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return CreatedAtAction(nameof(GetTemplateType), new { typeId = operationResult.Data.ReadableId }, _mapper.Map<TemplateTypeResponse>(operationResult.Data));
        }

        /// <summary>
        /// Disables the notification template type with the given Id, if there are no active templates with this Id.
        /// </summary>
        /// <param name="templateId">The Id of the <see cref="TemplateType"/> to disable.</param>
        /// <returns></returns>
        [HttpDelete, Route("types"), ProducesResponseType(204), ProducesResponseType(400)]
        public async Task<IActionResult> DeleteTemplateType([FromQuery] string templateId)
        {
            IOperationResult<bool> operationResult = await _templateTypeManager.DisableTemplateType(templateId);

            if (!operationResult.Success)
            {
                if (operationResult.ContainsError)
                    _logger.LogError(operationResult.ErrorDetail, operationResult.ErrorDetail.Message);
                return BadRequest(operationResult);
            }

            return NoContent();
        }
        #endregion
    }
}
