using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using Xunit.Sdk;

namespace todo_list_backend_tests
{
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void TestPasswordHashing()
        {
            var password = "password";

            var hashAndSalt = todo_list_backend.Utils.Authentication.GenerateHashAndSalt(password);
            var authAttempt = todo_list_backend.Utils.Authentication.VerifyPassword(hashAndSalt.Item1, hashAndSalt.Item2, password);

            Assert.IsTrue(authAttempt);
        }
    }
}
