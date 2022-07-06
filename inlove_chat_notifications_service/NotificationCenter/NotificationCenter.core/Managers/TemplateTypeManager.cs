using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Managers
{
    /// <summary>
    /// Represents a manager that's responsible of <see cref="TemplateType"/>s.
    /// </summary>
    public class TemplateTypeManager
    {
        private readonly ITemplateTypeStoreService _templateTypeStore;

        /// <summary>
        /// Creates a new instance of the <see cref="TemplateTypeManager"/>
        /// </summary>
        /// <param name="templateTypeStoreService">An implementation of the <see cref="ITemplateTypeStoreService"/> interface.</param>
        public TemplateTypeManager(ITemplateTypeStoreService templateTypeStoreService)
        {
            _templateTypeStore = templateTypeStoreService;
        }

        /// <summary>
        /// Gets a <see cref="TemplateType"/> with the provided id.
        /// </summary>
        /// <param name="templateTypeId">The Id to look for.</param>
        /// <returns>A <see cref="TemplateType"/> with the provided Id, if found.</returns>
        public async Task<IOperationResult<TemplateType>> GetTemplateType(string templateTypeId)
        {
            try
            {
                TemplateType result = await _templateTypeStore.GetTemplateType(templateTypeId);

                return result == null
                    ? BasicOperationResult<TemplateType>.Fail("No template type was found for the given ID")
                    : BasicOperationResult<TemplateType>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<TemplateType>.Fail("An error ocurred while trying to find a template type with the specified ID", ex);
            }
        }

        /// <summary>
        /// Returns all registered <see cref="TemplateType"/>s.
        /// </summary>
        /// <returns>A list of <see cref="TemplateType"/>s.</returns>
        public async Task<IOperationResult<List<TemplateType>>> GetAllTemplateTypes()
        {
            try
            {
                List<TemplateType> result = await _templateTypeStore.GetAllTemplateTypes();

                return result == null
                    ? BasicOperationResult<List<TemplateType>>.Fail("No template types were found")
                    : BasicOperationResult<List<TemplateType>>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<List<TemplateType>>.Fail("An error ocurred while trying to find template types", ex);
            }
        }

        /// <summary>
        /// Registers a new <see cref="TemplateType"/>.
        /// </summary>
        /// <param name="mappedTemplateType">The data of the template type to register.</param>
        /// <returns>The newly created <see cref="TemplateType"/> if successful.</returns>
        public async Task<IOperationResult<TemplateType>> CreateTemplateType(TemplateType mappedTemplateType)
        {
            try
            {
                bool templateTypeIdExists = await (_templateTypeStore.GetTemplateType(mappedTemplateType.ReadableId)) != null;

                if (templateTypeIdExists) return BasicOperationResult<TemplateType>.Fail("A template type with this Id already exists");

                TemplateType result = await _templateTypeStore.CreateTemplateType(mappedTemplateType);

                return result == null
                    ? BasicOperationResult<TemplateType>.Fail("Could not created this template type")
                    : BasicOperationResult<TemplateType>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<TemplateType>.Fail("An error ocurred while trying to create this template type", ex);
            }
        }

        /// <summary>
        /// Disables a <see cref="TemplateType"/>.
        /// </summary>
        /// <param name="templateTypeId">The Id of the template type to disable.</param>
        /// <param name="disableTemplates">
        /// Whether the currently active <see cref="NotificationTemplate"/>s of this type should be disabled.
        /// This operation will fail if there are active notification templates of this type, so they should be disabled before calling this method, or this must be true.
        /// </param>
        /// <returns>True if the template type was disabled correctly, false otherwise.</returns>
        public async Task<IOperationResult<bool>> DisableTemplateType(string templateTypeId, bool disableTemplates = false)
        {
            try
            {
                TemplateType templateType = await _templateTypeStore.GetTemplateType(templateTypeId);

                if (templateType == null) return BasicOperationResult<bool>.Fail("No template type was found for the given ID");

                if (disableTemplates)
                {
                    templateType.Templates.ForEach(template => template.IsActive = false);
                }
                else
                {
                    if (templateType.Templates.Any(template => template.IsActive))
                        return BasicOperationResult<bool>.Fail("This template type cannot be disabled until all its templates are disabled");
                }

                bool result = await _templateTypeStore.DisableTemplateType(templateTypeId);

                return result
                    ? BasicOperationResult<bool>.Fail("This template type could not be disabled")
                    : BasicOperationResult<bool>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<bool>.Fail("A problem occured trying to disable this template type", ex);
            }
        }
    }
}
