using Microsoft.EntityFrameworkCore;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Enums;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationCenter.Core
{
    ///<inheritdoc cref="INotificationTemplateStoreService"/>
    public class NotificationTemplateStoreService : INotificationTemplateStoreService
    {
        private readonly NotificationCenterContext _context;

        /// <summary>
        /// Creates a new instance of <see cref="NotificationTemplateStoreService"/>
        /// </summary>
        /// <param name="context">A <see cref="NotificationCenterContext"/> object</param>
        public NotificationTemplateStoreService(NotificationCenterContext context)
        {
            _context = context;
        }

        ///<inheritdoc/>
        public async Task<NotificationTemplateSendHistory> AddNotificationSendLogEntry(NotificationTemplateSendHistory notificationHistory)
        {
            await _context.NotificationTemplateHistory.AddAsync(notificationHistory);

            if (await _context.SaveChangesAsync() > 0)
                return notificationHistory;
            else return null;
        }

        ///<inheritdoc/>
        public async Task<NotificationTemplate> CreateNewNotificationTemplate(NotificationTemplate notificationTemplate)
        {
            await _context.NotificationTemplates.AddAsync(notificationTemplate);
            await _context.SaveChangesAsync();
            return notificationTemplate;
        }

        ///<inheritdoc/>
        public async Task<NotificationTemplate> GetNotificationTemplate(Guid notificationTemplateId) =>
              await _context.NotificationTemplates
                  .Include(template => template.TemplateType)
                  .ThenInclude(type => type.Data)
                  .FirstOrDefaultAsync(template => template.Id == notificationTemplateId);

        ///<inheritdoc/>
        public async Task<List<NotificationTemplate>> GetNotificationTemplatesByType(TemplateType notificationTemplateType) => await _context.NotificationTemplates
            .Where(template => template.IsActive && template.TemplateType == notificationTemplateType)
            .ToListAsync();

        ///<inheritdoc/>
        public async Task<PaginatedList<NotificationTemplateSendHistory>> GetSentNotificationsByUser(TokenUser user, int count, int page)
        {
            int itemCount = _context.NotificationTemplateHistory.Where(log => !log.Ignored && log.TokenUserId == user.Id).Count();

            List<NotificationTemplateSendHistory> items = await _context.NotificationTemplateHistory
                .Include(h => h.NotificationTemplate)
                .ThenInclude(t => t.TemplateType)
                .ThenInclude(t => t.Data)
                .Where(log => !log.Ignored && log.TokenUserId == user.Id)
                .OrderByDescending(log => log.DateSent)
                .Skip((page - 1) * count)
                .Take(count)
                .ToListAsync();

            return new PaginatedList<NotificationTemplateSendHistory>(items, itemCount, page, count);
        }

        ///<inheritdoc/>
        public async Task<NotificationTemplateSendHistory> MarkNotificationAsRead(Guid id, ReadMethod method)
        {
            NotificationTemplateSendHistory notificationTemplateLog = await _context.NotificationTemplateHistory.FindAsync(id);

            notificationTemplateLog.DateRead ??= DateTime.Now;
            notificationTemplateLog.ReadMethod = method;

            if (await _context.SaveChangesAsync() > 0) return notificationTemplateLog;
            else return null;
        }

        ///<inheritdoc/>
        public async Task<NotificationTemplateSendHistory> MarkNotificationAsIgnored(Guid id)
        {
            NotificationTemplateSendHistory notificationTemplateLog = await _context.NotificationTemplateHistory.FindAsync(id);

            notificationTemplateLog.Ignored = true;

            if (await _context.SaveChangesAsync() > 0) return notificationTemplateLog;
            else return null;
        }

        /// <inheritdoc/>
        public async Task<bool> MarkManyNotificationsAsRead(List<Guid> parsedNotificationsIds)
        {
            IQueryable<NotificationTemplateSendHistory> items = _context.NotificationTemplateHistory
                .Where(log => parsedNotificationsIds.Any(id => id == log.Id) && log.DateRead == null);

            if (items.Count() <= 0) return true;

            DateTime now = DateTime.Now;

            await items.ForEachAsync(item =>
            {
                if (!item.IsRead)
                {
                    item.DateRead = now;
                    item.ReadMethod = ReadMethod.Batch;
                }
            });

            return await _context.SaveChangesAsync() > 0;
        }

        /// <inheritdoc/>
        public async Task<bool> MarkManyNotificationsAsIgnored(List<Guid> parsedNotificationsIds)
        {
            IQueryable<NotificationTemplateSendHistory> items = _context.NotificationTemplateHistory.Where(log => parsedNotificationsIds.Any(id => id == log.Id));

            await items.ForEachAsync(item => item.Ignored = true);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
