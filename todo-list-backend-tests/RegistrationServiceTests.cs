using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using todo_list_backend.Models.Auth;
using todo_list_backend.Services;

namespace todo_list_backend_tests
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private IAuthTokenService authService;
        private IUserService userService;
        private RegistrationService service;

        [TestInitialize]
        public void Init()
        {
            authService = new MockTokenAuthService(MockAuthBehaviour.AcceptByDefault);
        }

        [TestMethod]
        public void Register_Email_Accepted_WhenAvailable()
        {
            userService = new MockUserService(true, false);
            service = new RegistrationService(userService, authService);

            var registerAttempt = service.RegisterWithEmailAndPassword(new EmailRegisterDto
            {
                DisplayName = TestData.user.DisplayName,
                Email = TestData.user.Email,
                Password = ""
            });

            Assert.IsTrue(registerAttempt.UserAvailable);
        }

        [TestMethod]
        public void Register_Email_Rejected_WhenNotAvailable()
        {
            userService = new MockUserService(true, true);
            service = new RegistrationService(userService, authService);

            var registerAttempt = service.RegisterWithEmailAndPassword(new EmailRegisterDto
            {
                DisplayName = TestData.user.DisplayName,
                Email = TestData.user.Email,
                Password = ""
            });

            Assert.IsFalse(registerAttempt.UserAvailable);
        }
    }
}
