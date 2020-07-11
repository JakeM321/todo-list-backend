using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Repositories;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IUserRepository _userRepository;
        private IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public AuthResult AuthenticateEmailAndPassword(string email, string password)
        {
            var search = _userRepository.Find(user => user.Email == email && !user.SsoUser);
            return search.Get(
                user => {
                    string secret = _configuration.GetValue("TodoListApp:JWTSecret", "");
                    var passwordMatches = Utils.Authentication.PasswordEncryption.VerifyPassword(user.PasswordHash, user.Salt, password);

                    return new AuthResult
                    {
                        Accepted = passwordMatches,
                        Token = passwordMatches ? BuildToken(secret, user.Id) : "",
                        User = user
                    };
                },
                () => new AuthResult {
                    Accepted = false,
                    Token = "",
                    User = null
                }
            );
        }

        public AuthResult AuthenticateToken(string token)
        {
            string secret = _configuration.GetValue("TodoListApp:JWTSecret", "");

            try
            {
                var decoded = Utils.Authentication.Jwt.ReadToken<UserToken>(token, secret);

                var fiveMinutesFromNow = DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds();

                return _userRepository.FindById(decoded.userId).Get(
                    user => new AuthResult
                    {
                        Accepted = true,
                        User = user,
                        Token = fiveMinutesFromNow > decoded.exp ? BuildToken(secret, user.Id) : token
                    },
                    () => new AuthResult { Accepted = false, Token = "", User = null }
                );
            }
            catch (Exception ex)
            {
                if (ex is TokenExpiredException || ex is SignatureVerificationException)
                {
                    Console.WriteLine("Failed auth");
                    return new AuthResult { Accepted = false, Token = "", User = null };
                }
                else
                {
                    throw ex;
                }
            }
        }

        private string BuildToken(string secret, int userId)
        {
            return Utils.Authentication.Jwt.BuildToken(new Dictionary<string, string> {
                { "exp", DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds().ToString() },
                { "userId", userId.ToString() }
            }, secret);
        }
    }
}
