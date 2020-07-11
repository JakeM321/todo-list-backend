﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public interface IAuthenticationService
    {
        public AuthResult AuthenticateEmailAndPassword(string email, string password);
        public AuthResult AuthenticateToken(string token);
    }
}
