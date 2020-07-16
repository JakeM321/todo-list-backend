using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Notification;
using todo_list_backend.Services;

namespace todo_list_backend.Repositories
{
    public class NotificationRepository: INotificationRepository
    {
        private AppDbContext _db;
        private ILogger _logger;

        public NotificationRepository(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger("User Respository");
        }        

        public UserNotificationRecord Save(UserNotificationRecord record)
        {
            var newRecord = _db.Add(record);
            _db.SaveChanges();
            return newRecord.Entity;
        }

        public IEnumerable<UserNotificationRecord> GetUserNotifications(int userId, int skip = 0, int take = 0)
        {
            return _db.Notifications.Where(record => record.UserId == userId).Skip(skip).Take(take);
        }
    }
}
