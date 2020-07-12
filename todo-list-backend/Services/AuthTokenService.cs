using JWT.Exceptions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public class AuthTokenService : IAuthTokenService
    {
        private IUserService _userService;
        private string _secret;

        public AuthTokenService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _secret = configuration.GetValue("TodoListApp:JWTSecret", "");
        }

        public AuthResult AuthenticateToken(string token)
        {
            try
            {
                var decoded = Utils.Authentication.Jwt.ReadToken<UserToken>(token, _secret);

                var refreshWindow = DateTimeOffset.UtcNow.AddMinutes(Constants.TOKEN_REFRESH_WINDOW_MINUTES).ToUnixTimeSeconds();

                return _userService.FindById(decoded.userId).Get(
                    user => new AuthResult
                    {
                        Accepted = true,
                        User = new Option<UserDto>(user),
                        Token = refreshWindow > decoded.exp ? GenerateToken(user.Id) : token
                    },
                    () => new AuthResult { Accepted = false, Token = "", User = null }
                );
            }
            catch (Exception ex)
            {
                if (ex is TokenExpiredException || ex is SignatureVerificationException)
                {
                    Console.WriteLine("Failed auth");
                    return new AuthResult { Accepted = false, Token = "", User = new Option<UserDto>() };
                }
                else
                {
                    throw ex;
                }
            }
        }

        public string GenerateToken(int userId)
        {
            return Utils.Authentication.Jwt.BuildToken(new Dictionary<string, string> {
                { "exp", DateTimeOffset.UtcNow.AddMinutes(Constants.TOKEN_LIFESPAN_MINUTES).ToUnixTimeSeconds().ToString() },
                { "userId", userId.ToString() }
            }, _secret);
        }
    }
}
