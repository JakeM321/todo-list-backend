using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Repositories;

namespace todo_list_backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool AuthenticateEmailAndPassword(string email, string password)
        {
            var search = _userRepository.Find(user => user.Email == email && !user.SsoUser);
            return search.Get(
                user => Utils.Authentication.VerifyPassword(user.PasswordHash, user.Salt, password),
                () => false
            );
        }
    }
}
