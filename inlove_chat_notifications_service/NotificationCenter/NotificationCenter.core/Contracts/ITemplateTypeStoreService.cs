using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents the contract for the template type store.
    /// </summary>
    public interface ITemplateTypeStoreService
    {
        /// <summary>
        /// Retrieves a <see cref="TemplateType"/> with the given Id
        /// </summary>
        /// <param name="id">The Id of the <see cref="TemplateType"/> to look for</param>
        /// <returns>The <see cref="TemplateType"/> with the given Id, if found.</returns>
        Task<TemplateType> GetTemplateType(string id);

        /// <summary>
        /// Retrieves all <see cref="TemplateType"/>s
        /// </summary>
        /// <returns>A list of <see cref="TemplateType"/>s</returns>
        Task<List<TemplateType>> GetAllTemplateTypes();

        /// <summary>
        /// Creates a new <see cref="TemplateType"/>.
        /// </summary>
        /// <param name="templateType">The <see cref="TemplateType"/> to create.</param>
        /// <returns>The newly created <see cref="TemplateType"/></returns>
        Task<TemplateType> CreateTemplateType(TemplateType templateType);

        /// <summary>
        /// Disables a <see cref="TemplateType"/>
        /// </summary>
        /// <param name="id">The Id of the <see cref="TemplateType"/> to disable.</param>
        /// <returns>True if the <see cref="TemplateType"/> was disabled correctly, false otherwise.</returns>
        Task<bool> DisableTemplateType(string id);
    }
}
