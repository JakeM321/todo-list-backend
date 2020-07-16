using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public interface IAuthTokenService
    {
        public AuthResult AuthenticateToken(string token);
        public string GenerateToken(int userId);
    }
}
