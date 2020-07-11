using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using todo_list_backend.Types;
using todo_list_backend.Utils;
using Xunit.Sdk;

namespace todo_list_backend_tests
{
    internal class TokenTestObject
    {
        public long tokenValue { get; set; }    
    }

    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void TestPasswordHashing()
        {
            var password = "password";

            var hashAndSalt = Authentication.PasswordEncryption.GenerateHashAndSalt(password);
            var authAttempt = Authentication.PasswordEncryption.VerifyPassword(hashAndSalt.Item1, hashAndSalt.Item2, password);

            Assert.IsTrue(authAttempt);
        }

        [TestMethod]
        public void TestTokenSerialization()
        {
            var secret = "7217979a-dcf2-49ff-83d1-45ddb7ae9f50";
            var tokenValue = 84359823450561;

            var token = Authentication.Jwt.BuildToken(new Dictionary<string, string> {
                { "tokenValue", tokenValue.ToString() }
            }, secret);

            var readAttempt = Authentication.Jwt.ReadToken<TokenTestObject>(token, secret);

            Assert.AreEqual(tokenValue, readAttempt.tokenValue);
        }
    }
}
