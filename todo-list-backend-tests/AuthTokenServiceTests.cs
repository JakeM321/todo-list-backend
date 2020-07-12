using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using todo_list_backend.Models.User;
using todo_list_backend.Services;
using todo_list_backend.Types;

namespace todo_list_backend_tests
{
    [TestClass]
    public class AuthTokenServiceTests
    {
        internal class MockUserService : IUserService
        {
            private UserDto user = new UserDto
            {
                Id = 500,
                DisplayName = "Test user",
                Email = "user@test.com",
                SsoUser = false
            };

            public bool ComparePassword(int userId, string password)
            {
                return true;
            }

            public UserDto CreateSsoUser(string email, string displayName)
            {
                return user;
            }

            public UserDto CreateUserWithPassword(string email, string displayName, string password)
            {
                return user;
            }

            public Option<UserDto> FindByEmail(string email)
            {
                return new Option<UserDto>(user);
            }

            public Option<UserDto> FindByEmailRegistration(string email)
            {
                return new Option<UserDto>(user);
            }

            public Option<UserDto> FindById(int id)
            {
                return new Option<UserDto>(user);
            }
        }

        private IConfiguration config;
        private IAuthTokenService authService;

        [TestInitialize]
        public void Init()
        {
            config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "TodoListApp:JWTSecret", Guid.NewGuid().ToString() } })
                .Build();

            authService = new AuthTokenService(new MockUserService(), config);
        }

        [TestMethod]
        public void AuthenticateToken_Accepts_Token_From_GenerateToken()
        {
            var token = authService.GenerateToken(500);
            var authAttempt = authService.AuthenticateToken(token);

            var returnedUserId = authAttempt.User.Get(u => u.Id, () => 0);

            Assert.IsTrue(authAttempt.Accepted);
            Assert.AreEqual(returnedUserId, 500);
        }

        [TestMethod]
        public void AuthenticateToken_Does_Not_Unnecessarily_Refresh_Token()
        {
            var token = authService.GenerateToken(500);
            var authAttempt = authService.AuthenticateToken(token);

            Assert.AreEqual(token, authAttempt.Token);
        }

        [TestMethod]
        public void Service_Refreshes_Token_When_Needed()
        {
            var now = DateTimeOffset.UtcNow;

            var lifespan = todo_list_backend.Constants.TOKEN_LIFESPAN_MINUTES;
            var window = todo_list_backend.Constants.TOKEN_REFRESH_WINDOW_MINUTES;

            //Middle of the refresh window
            var timeToSubtract = (lifespan - window) + window / 2;

            var tokenThatExpiresSoon = todo_list_backend.Utils.Authentication.Jwt.BuildToken(new Dictionary<string, string> {
                { "exp", now.AddMinutes(0 - timeToSubtract).ToUnixTimeSeconds().ToString() },
                { "userId", "500" }
            }, config.GetValue<string>("TodoListApp:JWTSecret"));

            var notAboutToExpire = todo_list_backend.Utils.Authentication.Jwt.BuildToken(new Dictionary<string, string> {
                { "exp", now.AddHours(5).ToUnixTimeSeconds().ToString() },
                { "userId", "500" }
            }, config.GetValue<string>("TodoListApp:JWTSecret"));

            var shouldReset = authService.AuthenticateToken(tokenThatExpiresSoon);
            var shouldNotReset = authService.AuthenticateToken(notAboutToExpire);

            Assert.AreNotEqual(tokenThatExpiresSoon, shouldReset.Token);
            Assert.AreEqual(notAboutToExpire, shouldNotReset.Token);
        }
    }
}
