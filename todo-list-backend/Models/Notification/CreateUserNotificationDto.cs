﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Notification
{
    public class CreateUserNotificationDto : CreateNotificationDto
    {
        public int UserId { get; set; }
    }
}
