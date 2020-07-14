using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;

namespace todo_list_backend.SignalR
{
    public interface INotificationHubManager
    {
        void RegisterConnection(int userId, string connectionId);
        Task SendNotificationAsync(SendUserNotificationDto notification, int millisecondsDelay = 0);
    }
}
