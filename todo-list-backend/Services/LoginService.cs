using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
    public class LoginService : ILoginService
    {
        private IUserService _userService;
        private IAuthTokenService _authTokenService;

        public LoginService(IUserService userService, IAuthTokenService authTokenService)
        {
            _userService = userService;
            _authTokenService = authTokenService;
        }

        public AuthResult AuthenticateEmailAndPassword(EmailLoginDto payload)
        {
            var search = _userService.FindByEmailRegistration(payload.Email);
            return search.Get(
                user => {
                    var passwordMatches = _userService.ComparePassword(user.Id, payload.Password);

                    return new AuthResult
                    {
                        Accepted = passwordMatches,
                        Token = passwordMatches ? _authTokenService.GenerateToken(user.Id) : "",
                        User = new Option<UserDto>(user)
                    };
                },
                () => new AuthResult {
                    Accepted = false,
                    Token = "",
                    User = new Option<UserDto>()
                }
            );
        }
    }
}
