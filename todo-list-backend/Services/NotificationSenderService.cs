using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.SignalR;

namespace todo_list_backend.Services
{
    public class NotificationSenderService : INotificationSenderService
    {
        private INotificationHubManager _notificationHubManager;
        private INotificationRepository _notificationRespository;

        public NotificationSenderService(INotificationHubManager notificationHubManager, INotificationRepository notificationRepository)
        {
            _notificationHubManager = notificationHubManager;
            _notificationRespository = notificationRepository;
        }
        public Task SendNotificationAsync(CreateUserNotificationDto notification, int millsecondsDelay = 0)
        {
            var record = _notificationRespository.Save(new UserNotificationRecord
            {
                UserId = notification.UserId,
                Subject = notification.Subject,
                Message = notification.Message,
                isLink = notification.isLink,
                Link = notification.Link,
                Seen = false
            });

            return _notificationHubManager.SendNotificationAsync(new SendUserNotificationDto {
                Id = record.Id,
                UserId = record.UserId,
                Subject = record.Subject,
                Message = record.Message,
                isLink = record.isLink,
                Link = record.Link,
                Seen = record.Seen
            }, millsecondsDelay);
        }
    }
}
