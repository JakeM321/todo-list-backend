﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;

namespace todo_list_backend.Services.Notifications
{
    public class NotificationProviderService : INotificationProviderService
    {
        private INotificationRepository _notificationRepository;
        public NotificationProviderService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<SendUserNotificationDto> GetUserNotifications(int userId, int skip, int take)
        {
            return _notificationRepository
                .GetUserNotifications(record => record.UserId == userId)
                .OrderBy(record => record.Id)
                .Skip(skip)
                .Take(take)
                .Select(record => new SendUserNotificationDto(record));
        }
    }
}