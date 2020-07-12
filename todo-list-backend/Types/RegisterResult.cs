using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models;
using todo_list_backend.Models.User;

namespace todo_list_backend.Types
{
    public class RegisterResult
    {
        public bool UserAvailable { get; set; }
        public string Token { get; set; }
        public Option<UserDto> User { get; set; }
    }
}
