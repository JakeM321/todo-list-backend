using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.Auth
{
    public class EmailLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
