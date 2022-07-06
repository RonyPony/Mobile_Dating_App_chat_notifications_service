using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents the contract for the notification template store
    /// </summary>
    public interface INotificationTemplateStoreService
    {
        /// <summary>
        /// Retrieves a <see cref="NotificationTemplate"/> with the given Id.
        /// </summary>
        /// <param name="notificationTemplateId">The ID of the notification template to look for</param>
        /// <returns>A notification template with the given ID, if found.</returns>
        Task<NotificationTemplate> GetNotificationTemplate(Guid notificationTemplateId);

        /// <summary>
        /// Creates a new <see cref="NotificationTemplate"/>.
        /// </summary>
        /// <param name="notificationTemplate">The <see cref="NotificationTemplate"/> to register.</param>
        /// <returns>The created notification template, if successful.</returns>
        Task<NotificationTemplate> CreateNewNotificationTemplate(NotificationTemplate notificationTemplate);

        /// <summary>
        /// Get all the <see cref="NotificationTemplate"/>s for the given <see cref="TemplateType"/>.
        /// </summary>
        /// <param name="notificationTemplateType">The <see cref="TemplateType"/> from which the templates are to be searched.</param>
        /// <returns>A list of <see cref="NotificationTemplate"/>s.</returns>
        Task<List<NotificationTemplate>> GetNotificationTemplatesByType(TemplateType notificationTemplateType);

        /// <summary>
        /// Marks a sent notification as read.
        /// </summary>
        /// <param name="id">The Id of the <see cref="NotificationTemplateSendHistory"/> corresponding to the notification sent.</param>
        /// <param name="method">Indicates the way this notification was read</param>
        /// <returns>True if the notification could be marked as read, false otherwise</returns>
        Task<NotificationTemplateSendHistory> MarkNotificationAsRead(Guid id, Enums.ReadMethod method);

        /// <summary>
        /// Adds a log entry of a notification sent.
        /// </summary>
        /// <param name="notificationHistory"> A <see cref="NotificationTemplateSendHistory"/> representing this log.</param>
        /// <returns>The created <see cref="NotificationTemplateSendHistory"/></returns>
        Task<NotificationTemplateSendHistory> AddNotificationSendLogEntry(NotificationTemplateSendHistory notificationHistory);

        /// <summary>
        /// Gets all notifications sent to an user
        /// </summary>
        /// <param name="user">The user who's notifications will be fetched</param>
        /// <param name="count">The number of items per page</param>
        /// <param name="page">The number of the page to get</param>
        /// <returns>A list of notifications</returns>
        Task<PaginatedList<NotificationTemplateSendHistory>> GetSentNotificationsByUser(TokenUser user, int count, int page);

        /// <summary>
        /// Marks a sent notification as ignored.
        /// </summary>
        /// <param name="id">The Id of the <see cref="NotificationTemplateSendHistory"/> corresponding to the notification sent.</param>
        /// <returns>True if the notification could be marked as ignored, false otherwise</returns>
        Task<NotificationTemplateSendHistory> MarkNotificationAsIgnored(Guid id);

        /// <summary>
        /// Marks many notifications as read at once.
        /// </summary>
        /// <param name="parsedNotificationsIds">A list of the ids of the notifications to be marked as read.</param>
        /// <returns>True if any of the notifications could be marked, false otherwise.</returns>
        Task<bool> MarkManyNotificationsAsRead(List<Guid> parsedNotificationsIds);

        /// <summary>
        /// Marks many notifications as ignored at once.
        /// </summary>
        /// <param name="parsedNotificationsIds">A list of the ids of the notifications to be marked as ignored.</param>
        /// <returns>True if any of the notifications could be marked, false otherwise.</returns>
        Task<bool> MarkManyNotificationsAsIgnored(List<Guid> parsedNotificationsIds);
    }
}
