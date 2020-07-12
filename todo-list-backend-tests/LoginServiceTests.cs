using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using todo_list_backend.Models.Auth;
using todo_list_backend.Services;

namespace todo_list_backend_tests
{
    [TestClass]
    public class LoginServiceTests
    {
        private IAuthTokenService authService;
        private IUserService userService;
        private LoginService service;

        [TestInitialize]
        public void Init()
        {
            userService = new MockUserService();
        }

        [TestMethod]
        public void Login_Works_When_Accepted()
        {
            authService = new MockTokenAuthService(MockAuthBehaviour.AcceptByDefault);
            service = new LoginService(userService, authService);

            var loginAttempt = service.AuthenticateEmailAndPassword(new EmailLoginDto {
                Email = TestData.user.Email,
                Password = ""
            });

            Assert.IsTrue(loginAttempt.Accepted);
        }

        [TestMethod]
        public void Login_Fails_When_Rejected()
        {
            userService = new MockUserService(false);
            authService = new MockTokenAuthService(MockAuthBehaviour.RejectByDefault);
            service = new LoginService(userService, authService);

            var loginAttempt = service.AuthenticateEmailAndPassword(new EmailLoginDto
            {
                Email = TestData.user.Email,
                Password = ""
            });

            Assert.IsFalse(loginAttempt.Accepted);
        }
    }
}
