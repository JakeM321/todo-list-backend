using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;

namespace todo_list_backend.SignalR
{
    public class NotificationHubManager : INotificationHubManager
    {
        private Dictionary<int, string> _connectedUsers;
        private IHubContext<NotificationHub> _hubContext;

        public NotificationHubManager(IHubContext<NotificationHub> hubContext)
        {
            _connectedUsers = new Dictionary<int, string>();
            _hubContext = hubContext;
        }

        public void RegisterConnection(int userId, string connectionId)
        {
            if (_connectedUsers.ContainsKey(userId))
            {
                _connectedUsers.Remove(userId);
            }

            _connectedUsers.Add(userId, connectionId);
        }

        public async Task SendNotificationAsync(SendUserNotificationDto notification, int millisecondsDelay = 0)
        {
            await Task.Delay(millisecondsDelay);

            if (_connectedUsers.ContainsKey(notification.UserId))
            {
                await _hubContext.Clients.Client(_connectedUsers[notification.UserId]).SendAsync("notification", notification);
            }
            else
            {
                await Task.CompletedTask;
            }
        }
    }
}
