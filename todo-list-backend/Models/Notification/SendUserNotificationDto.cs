using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Notification
{
    public class SendUserNotificationDto : UserNotificationRecord
    {
        public SendUserNotificationDto(UserNotificationRecord record)
        {
            Id = record.Id;
            UserId = record.UserId;
            Subject = record.Subject;
            Message = record.Message;
            isLink = record.isLink;
            Link = record.Link;
            Seen = record.Seen;
        }
    }
}
