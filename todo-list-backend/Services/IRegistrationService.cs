using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Auth;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public interface IRegistrationService
    {
        public RegisterResult RegisterWithEmailAndPassword(EmailRegisterDto payload);
    }
}
