using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Models.User;
using todo_list_backend.Services;

namespace todo_list_backend.SignalR
{
    public class NotificationHub : Hub
    {
        private INotificationHubManager _manager;
        private INotificationRepository _notificationRepository;
        public NotificationHub(INotificationHubManager manager, INotificationRepository notificationRepository)
        {
            _manager = manager;
            _notificationRepository = notificationRepository;
        }

        private async Task SendInitialNotifications(SendUserNotificationDto[] notifications)
        {
            foreach (var notification in notifications)
            {
                await _manager.SendNotificationAsync(notification);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var user = (UserDto)this.Context.GetHttpContext().Items["user"];
            _manager.RegisterConnection(user.Id, Context.ConnectionId);

            var notifications = _notificationRepository
                .GetUserNotifications(user.Id, 0, 20)
                .OrderBy(n => n.Id)
                .Select(record => new SendUserNotificationDto(record))
                .ToArray();

            await base.OnConnectedAsync();
            await SendInitialNotifications(notifications);
        }
    }
}
