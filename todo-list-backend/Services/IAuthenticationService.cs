using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Services
{
    public interface IAuthenticationService
    {
        public bool AuthenticateEmailAndPassword(string email, string password);
    }
}
