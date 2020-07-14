using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;

namespace todo_list_backend.Services
{
    public interface INotificationProviderService
    {
        IEnumerable<SendUserNotificationDto> GetUserNotifications(int userId);
    }
}
