using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Auth;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public class OAuthHandlerService : IOAuthHandlerService
    {
        private IUserService _userService;
        private IAuthTokenService _authTokenService;

        public OAuthHandlerService(IUserService userService, IAuthTokenService authTokenService)
        {
            _userService = userService;
            _authTokenService = authTokenService;
        }

        private AuthResult Authenticate(UserDto user)
        {
            return new AuthResult
            {
                Accepted = true,
                Token = _authTokenService.GenerateToken(user.Id),
                User = new Option<UserDto>(user)
            };
        }

        public AuthResult LoginOrRegister(OAuthUserInfo user)
        {
            return _userService.FindByEmail(user.Email).Get(existingUser => Authenticate(existingUser), () =>
            {
                var newUser = _userService.CreateSsoUser(user.Email, String.Format("{0} {1}", user.FirstName, user.LastName));
                return Authenticate(newUser);
            });
        }
    }
}
