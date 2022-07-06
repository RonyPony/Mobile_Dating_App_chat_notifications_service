using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationCenter.Core
{
    ///<inheritdoc cref="ITemplateTypeStoreService"/>
    public class TemplateTypeStoreService : ITemplateTypeStoreService
    {
        private readonly NotificationCenterContext _context;

        /// <summary>
        /// Creates a new instance of <see cref="NotificationTemplateStoreService"/>
        /// </summary>
        /// <param name="context">A <see cref="NotificationCenterContext"/> object</param>
        public TemplateTypeStoreService(NotificationCenterContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public async Task<TemplateType> CreateTemplateType(TemplateType templateType)
        {
            bool templateTypeIdExists = await GetTemplateType(templateType.ReadableId) != null;

            //TODO
            if (templateTypeIdExists) throw new ArgumentException();

            _context.NotificationTemplateTypes.Add(templateType);

            if (await _context.SaveChangesAsync() > 0) return templateType;
            else return null;
        }

        ///<inheritdoc/>
        public async Task<bool> DisableTemplateType(string templateTypeId)
        {
            TemplateType templateType = await GetTemplateType(templateTypeId);

            //TODO
            if (templateType == null) throw new FormatException();

            templateType.IsActive = false;

            return await _context.SaveChangesAsync() > 0;
        }

        ///<inheritdoc/>
        public async Task<List<TemplateType>> GetAllTemplateTypes() => await _context.NotificationTemplateTypes
                .Include(t => t.Data)
                .Include(type => type.Templates)
                .Where(type => type.IsActive)
                .ToListAsync();

        ///<inheritdoc/>
        public async Task<TemplateType> GetTemplateType(string templateTypeId) => await _context.NotificationTemplateTypes
                .Include(t => t.Data)
                .Include(type => type.Templates)
                .FirstOrDefaultAsync(type => type.IsActive && type.ReadableId == templateTypeId);
    }
}
