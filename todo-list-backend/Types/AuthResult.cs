﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;

namespace todo_list_backend.Types
{
    public class AuthResult
    {
        public bool Accepted { get; set; }
        public string Token { get; set; }
        public Option<UserDto> User { get; set; }
    }
}
