﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Auth
{
    public class EmailLoginPayload
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}
