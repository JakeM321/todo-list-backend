using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models;
using todo_list_backend.Models.Auth;
using todo_list_backend.Models.User;
using todo_list_backend.Repositories;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public class RegistrationService : IRegistrationService
    {
        private IUserService _userService;
        private IAuthTokenService _authTokenService;

        public RegistrationService(IUserService userService, IAuthTokenService authTokenService)
        {
            _userService = userService;
            _authTokenService = authTokenService;
        }

        public RegisterResult RegisterWithEmailAndPassword(EmailRegisterDto payload)
        {
            return _userService.FindByEmail(payload.Email).Get(
                some => new RegisterResult
                {
                    UserAvailable = false,
                    Token = "",
                    User = new Option<UserDto>()
                },
                () =>
                {
                    var user = _userService.CreateUserWithPassword(payload.Email, payload.DisplayName, payload.Password);

                    var token = _authTokenService.GenerateToken(user.Id);

                    return new RegisterResult
                    {
                        UserAvailable = true,
                        User = new Option<UserDto>(user),
                        Token = token
                    };
                }
            );
        }
    }
}
