using AutoMapper;
using FirebaseAdmin.Messaging;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Enums;
using NotificationCenter.Core.Extensions;
using NotificationCenter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationCenter.Core.Managers
{
    /// <summary>
    /// Represents a manager that's responsible of <see cref="NotificationTemplate"/>s.
    /// </summary>
    public class NotificationTemplateManager
    {
        private const string ClickToAction = "FLUTTER_NOTIFICATION_CLICK";

        private readonly IDeviceStoreService _deviceService;
        private readonly ITokenUserStoreService _tokenUserService;
        private readonly ITemplateTypeStoreService _templateTypeService;
        private readonly INotificationTemplateStoreService _notificationTemplateStore;
        private readonly FirebaseMessaging _firebaseMessaging;
        private readonly IAppService _appService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of the <see cref="NotificationsManager"/>
        /// </summary>
        /// <param name="pushNotifierService">An implementation of <see cref="IPushNotifierService"/></param>
        /// <param name="notificationTemplateStore">An implementation of <see cref="INotificationTemplateStoreService"/></param>
        /// <param name="tokenUserService">An implementation of <see cref="ITokenUserStoreService"/></param>
        /// <param name="deviceService">An implementation of <see cref="IDeviceStoreService"/></param>
        public NotificationTemplateManager(
            ITemplateTypeStoreService templateTypeStoreService,
            INotificationTemplateStoreService notificationTemplateStore,
            ITokenUserStoreService tokenUserService,
            IDeviceStoreService deviceService, FirebaseMessaging firebaseMessaging, IAppService appService, IMapper mapper)
        {
            _templateTypeService = templateTypeStoreService;
            _notificationTemplateStore = notificationTemplateStore;
            _tokenUserService = tokenUserService;
            _deviceService = deviceService;
            _firebaseMessaging = firebaseMessaging;
            _appService = appService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a <see cref="NotificationTemplate"/> by Id.
        /// </summary>
        /// <param name="templateId">the NotificationTemplate Id to look for.</param>
        /// <returns>An <see cref="IOperationResult{NotificationTemplate}"/> completed with the found <see cref="NotificationTemplate"/>,or a failed <see cref="IOperationResult"/> if no results are found.</returns>
        public async Task<IOperationResult<NotificationTemplate>> GetNotificationTemplate(Guid templateId)
        {
            try
            {
                NotificationTemplate result = await _notificationTemplateStore.GetNotificationTemplate(templateId);

                return result == null
                    ? BasicOperationResult<NotificationTemplate>.Fail("No template was found for the given ID")
                    : BasicOperationResult<NotificationTemplate>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<NotificationTemplate>.Fail("An error ocurred while trying to find a template with the specified ID", ex);
            }
        }

        /// <summary>
        /// Creates a new <see cref="NotificationTemplate"/>.
        /// </summary>
        /// <param name="template">The template to create.</param>
        /// <returns>An <see cref="IOperationResult{NotificationTemplate}"/> completed with the created template, if successful.</returns>
        public async Task<IOperationResult<NotificationTemplate>> CreateNotificationTemplate(NotificationTemplateRequest template)
        {
            try
            {
                TemplateType templateType = await _templateTypeService.GetTemplateType(template.TemplateTypeId);

                if (templateType == null) return BasicOperationResult<NotificationTemplate>.Fail("A valid template type Id must be provided");

                NotificationTemplate mappedTemplate = new NotificationTemplate
                {
                    Title = template.Title,
                    Body = template.Body,

                    HighPriority = template.HighPriority,
                    TemplateType = templateType,
                };

                NotificationTemplate result = await _notificationTemplateStore.CreateNewNotificationTemplate(mappedTemplate);

                return result == null
                    ? BasicOperationResult<NotificationTemplate>.Fail("No template was found for the given ID")
                    : BasicOperationResult<NotificationTemplate>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<NotificationTemplate>.Fail("An error ocurred while trying to create this template", ex);
            }
        }

        /// <summary>
        /// Sends a push notification to the devices of the specified user, with a payload built from the data provided, and based on a <see cref="NotificationTemplate"/>
        /// </summary>
        /// <param name="templateId">The Id of the <see cref="NotificationTemplate"/> to use for this notification</param>
        /// <param name="userId">The user that will receive this notification</param>
        /// <param name="appName">The package name of the app to which the notification will be send.</param>
        /// <param name="data">The data that will be used to build the notification payload and configure the notification template</param>
        /// <param name="readUrl">The url to call to mark this notification as read.</param>
        /// <returns>The <see cref="NotificationTemplateSendHistory"/> log corresponding to the notification sent.</returns>
        public async Task<IOperationResult<NotificationTemplateSendHistory>> SendNotification(Guid templateId, string userId, string appName, IDictionary<string, dynamic> data, string readUrl)
        {
            try
            {
                // Get the template
                NotificationTemplate template = await _notificationTemplateStore.GetNotificationTemplate(templateId);

                if (template == null)
                    return BasicOperationResult<NotificationTemplateSendHistory>.Fail("No template was found with the specified ID");

                // Get the user
                TokenUser user = await _tokenUserService.GetTokenUserByUserId(userId);

                if (user == null)
                    return BasicOperationResult<NotificationTemplateSendHistory>.Fail("No user was found with the specified ID");

                // Get the app
                App app = await _appService.GetAppByName(appName);

                // Get the user's devices
                ISet<Device> devices = (await _deviceService.GetDevicesForUserByTokenUserId(user.Id))
                    .Where(device => device.App.AppName == app.AppName)
                    .ToHashSet();

                if (devices.Count <= 0)
                    return BasicOperationResult<NotificationTemplateSendHistory>.Fail("The specified user has no registered devices");

                // Check if all required params were provided
                List<NotificationDataDetails> missingParams = CheckParams(template.TemplateType.Data, data);

                if (missingParams.Count > 0)
                    return BasicOperationResult<NotificationTemplateSendHistory>.Fail($"The following parameters where not provided: {string.Join(", ", missingParams.Select(param => param.Key))}");

                // Insert params where needed
                string notificationTitle = template.Title;
                string notificationBody = template.Body;

                foreach (var item in template.TemplateType.Data)
                {
                    notificationTitle = notificationTitle.Replace($"{{{{{item.Key}}}}}", item.Value);
                    notificationBody = notificationBody.Replace($"{{{{{item.Key}}}}}", item.Value);
                }

                // Build the notification log
                var notificationHistory = new NotificationTemplateSendHistory
                {
                    NotificationTemplate = template,
                    TokenUser = user,
                    DevicesCount = user.Devices.Count(),
                    DateSent = DateTime.Now,
                    Payload = JsonSerializer.Serialize(data)
                };

                // Build the notification payload
                IDictionary<string, string> notificationPayload = BuildNotificationPayload(template, notificationHistory, data, notificationTitle, notificationBody, user, app, readUrl);

                // Build the notification
                var notification = new MulticastMessage
                {
                    Data = (IReadOnlyDictionary<string, string>)notificationPayload,
                    Tokens = devices.Select(d => d.FcmToken).ToList(),
                };

                // Send the notification
                BatchResponse fcmresponse = await _firebaseMessaging.SendMulticastAsync(notification);

                // Update the notification sending operation result in the log
                notificationHistory.DeveicesSentTo = fcmresponse.SuccessCount;
                notificationHistory.FailedDevices = fcmresponse.FailureCount;

                // Save the notification log
                NotificationTemplateSendHistory log = await _notificationTemplateStore.AddNotificationSendLogEntry(notificationHistory);

                return BasicOperationResult<NotificationTemplateSendHistory>.Ok(log);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<NotificationTemplateSendHistory>.Fail("An error ocurred while trying to send a notification", ex);
            }
        }

        /// <summary>
        /// Marks many notifications as read.
        /// </summary>
        /// <param name="parsedNotificationsIds">A list of the ids of the notifications to mark as read</param>
        /// <returns>True if any of the notifications could be marked as read, false otherwise</returns>
        public async Task<IOperationResult<bool>> MarkManyNotificationsAsRead(List<Guid> parsedNotificationsIds)
        {
            try
            {
                bool result = await _notificationTemplateStore.MarkManyNotificationsAsRead(parsedNotificationsIds);

                return !result
                      ? BasicOperationResult<bool>.Fail("An error ocurred while trying to mark the notifications as read")
                      : BasicOperationResult<bool>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<bool>.Fail("An error ocurred while trying to mark the notifications as read", ex);
            }
        }

        /// <summary>
        /// Marks many notifications as ignored.
        /// </summary>
        /// <param name="parsedNotificationsIds">A list of the ids of the notifications to mark as ignored</param>
        /// <returns>True if any of the notifications could be marked as ignored, false otherwise</returns>
        public async Task<IOperationResult<bool>> MarkManyNotificationsAsIgnored(List<Guid> parsedNotificationsIds)
        {
            try
            {
                bool result = await _notificationTemplateStore.MarkManyNotificationsAsIgnored(parsedNotificationsIds);

                return !result
                      ? BasicOperationResult<bool>.Fail("An error ocurred while trying to mark the notifications as ignored")
                      : BasicOperationResult<bool>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<bool>.Fail("An error ocurred while trying to mark the notifications as ignored", ex);
            }
        }

        /// <summary>
        /// Gets all notifications sent to an user
        /// </summary>
        /// <param name="userId">The Id of the user who's notifications will be fetched</param>
        /// <param name="count">The number of items per page</param>
        /// <param name="page">The number of the page to get</param>
        /// <returns>A <see cref="PaginatedList{T}"/> with the notifications found.</returns>
        public async Task<IOperationResult<PaginatedList<Models.Notification>>> GetSentNotificationsByUser(string userId, int count, int page)
        {
            try
            {
                // Get the user
                TokenUser user = await _tokenUserService.GetTokenUserByUserId(userId);

                if (user == null)
                    return BasicOperationResult<PaginatedList<Models.Notification>>.Fail("No user was found with the specified ID");

                // Get the notifications
                PaginatedList<NotificationTemplateSendHistory> result = await _notificationTemplateStore.GetSentNotificationsByUser(user, count, page);

                DateTime now = DateTime.Now;

                // Convert the notifications
                PaginatedList<Models.Notification> parsedResult = result.Select(log =>
                {

                    string notificationTitle = log.NotificationTemplate.Title;
                    string notificationBody = log.NotificationTemplate.Body;

                    IDictionary<string, dynamic> data = JsonSerializer.Deserialize<IDictionary<string, dynamic>>(log.Payload);

                    log.NotificationTemplate.TemplateType.Data.ForEach(item =>
                    {
                        var pair = data.FirstOrDefault(d => d.Key == item.Key);
                        if (pair.Value?.ToString() != null)
                            item.Value = pair.Value.ToString();
                    });

                    foreach (var item in log.NotificationTemplate.TemplateType.Data)
                    {
                        notificationTitle = notificationTitle.Replace($"{{{{{item.Key}}}}}", item.Value);
                        notificationBody = notificationBody.Replace($"{{{{{item.Key}}}}}", item.Value);
                    }

                    return new Models.Notification
                    {
                        Id = log.Id,
                        Body = notificationBody,
                        Title = notificationTitle,
                        DateSent = log.DateSent,
                        DateRead = log.DateRead,
                        Payload = log.Payload,
                        Ignored = log.Ignored,
                        ReadMethod = log.ReadMethod,
                        NotificationTemplateId = log.NotificationTemplateId,
                        MinutesSinceSent = (int)Math.Round((now - log.DateSent).TotalMinutes)
                    };
                });

                return BasicOperationResult<PaginatedList<Models.Notification>>.Ok(parsedResult ?? new PaginatedList<Models.Notification>());
            }
            catch (Exception ex)
            {
                return BasicOperationResult<PaginatedList<Models.Notification>>.Fail("An error ocurred while getting the notifications.", ex);
            }
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="id">The Id <see cref="NotificationTemplateSendHistory"/> log corresponding to the notification sent.</param>
        /// <returns>The <see cref="NotificationTemplateSendHistory"/> log corresponding to the notification sent with updated data.</returns>
        public async Task<IOperationResult<NotificationTemplateSendHistory>> MarkNotificationAsRead(Guid id, ReadMethod method)
        {
            try
            {
                NotificationTemplateSendHistory result = await _notificationTemplateStore.MarkNotificationAsRead(id, method);

                return result == null
                      ? BasicOperationResult<NotificationTemplateSendHistory>.Fail("No notification record was found for the given ID")
                      : BasicOperationResult<NotificationTemplateSendHistory>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<NotificationTemplateSendHistory>.Fail("An error ocurred while trying to mark the notification as read", ex);
            }
        }

        /// <summary>
        /// Marks a notification as ignored.
        /// </summary>
        /// <param name="id">The Id <see cref="NotificationTemplateSendHistory"/> log corresponding to the notification sent.</param>
        /// <returns>The <see cref="NotificationTemplateSendHistory"/> log corresponding to the notification sent with updated data.</returns>
        public async Task<IOperationResult<NotificationTemplateSendHistory>> MarkNotificationAsIgnored(Guid id)
        {
            try
            {
                NotificationTemplateSendHistory result = await _notificationTemplateStore.MarkNotificationAsIgnored(id);

                return result == null
                      ? BasicOperationResult<NotificationTemplateSendHistory>.Fail("No notification record was found for the given ID")
                      : BasicOperationResult<NotificationTemplateSendHistory>.Ok(result);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<NotificationTemplateSendHistory>.Fail("An error ocurred while trying to mark the notification as ignored", ex);
            }
        }

        #region Private methods
        private IDictionary<string, string> BuildNotificationPayload(NotificationTemplate template, NotificationTemplateSendHistory notificationHistory, IDictionary<string, dynamic> data, string notificationTitle, string notificationBody, TokenUser user, App app, string readUrl)
        {
            IDictionary<string, string> notificationPayload = new Dictionary<string, string>();

            foreach (var item in data)
            {
                notificationPayload.Add(item.Key, item.Value.ToString());
            }

            notificationPayload.Add("notification_id", notificationHistory.Id.ToString());
            notificationPayload.Add("template_id", template.Id.ToString());
            notificationPayload.Add("read_url", readUrl + notificationHistory.Id.ToString());
            notificationPayload.Add("channel_id", "high_importance_channel");
            notificationPayload.Add("user_id", user.UserId);
            notificationPayload.Add("app_name", app.AppName);
            notificationPayload.Add("data_message_title", notificationTitle);
            notificationPayload.Add("data_message_body", notificationBody);

            return notificationPayload;
        }

        private List<NotificationDataDetails> CheckParams(List<NotificationDataDetails> templateParams, IDictionary<string, dynamic> data)
        {
            List<NotificationDataDetails> missing = new List<NotificationDataDetails>();

            foreach (var item in templateParams)
            {
                if (data.Any(key => key.Key.ToLower() == item.Key.ToLower()))
                    item.Value = data.FirstOrDefault(key => key.Key.ToLower() == item.Key.ToLower()).Value.ToString();
                else
                    missing.Add(item);
            }

            return missing;
        }
        #endregion
    }
}
