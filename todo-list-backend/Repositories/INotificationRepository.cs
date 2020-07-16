using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;

namespace todo_list_backend.Services
{
    public interface INotificationRepository
    {
        UserNotificationRecord Save(UserNotificationRecord record);
        IEnumerable<UserNotificationRecord> GetUserNotifications(int userId, int skip, int take);
    }
}
