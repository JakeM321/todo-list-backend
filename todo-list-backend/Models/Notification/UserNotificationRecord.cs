using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Notification
{
    public class UserNotificationRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool isLink { get; set; }
        public string Link { get; set; }
        public bool Seen { get; set; }
    }
}
