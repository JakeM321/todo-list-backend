using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using todo_list_backend.Models.User;
using todo_list_backend.Services;
using todo_list_backend.Types;

namespace todo_list_backend_tests
{
    public static class TestData
    {
        public static UserDto user = new UserDto
        {
            Id = 500,
            DisplayName = "Test user",
            Email = "user@test.com",
            SsoUser = false
        };
    }

    public class MockUserService : IUserService
    {
        private UserDto user = TestData.user;
        private bool _acceptPassword;
        private bool _returnDefaultUser;

        public MockUserService(bool acceptPassword = true, bool returnDefaultUser = true)
        {
            _acceptPassword = acceptPassword;
            _returnDefaultUser = returnDefaultUser;
        }

        public bool ComparePassword(int userId, string password)
        {
            return _acceptPassword;
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
            return _returnDefaultUser ? new Option<UserDto>(user) : new Option<UserDto>();
        }

        public Option<UserDto> FindByEmailRegistration(string email)
        {
            return _returnDefaultUser ? new Option<UserDto>(user) : new Option<UserDto>();
        }

        public Option<UserDto> FindById(int id)
        {
            return _returnDefaultUser ? new Option<UserDto>(user) : new Option<UserDto>();
        }
    }

    public enum MockAuthBehaviour
    {
        AcceptByDefault,
        RejectByDefault
    }

    public class MockTokenAuthService : IAuthTokenService
    {
        private MockAuthBehaviour _behaviour;
        public MockTokenAuthService(MockAuthBehaviour behaviour = MockAuthBehaviour.AcceptByDefault) {
            _behaviour = behaviour;
        }

        public AuthResult AuthenticateToken(string token)
        {
            return new AuthResult
            {
                Accepted = _behaviour == MockAuthBehaviour.AcceptByDefault ? true : false,
                Token = "",
                User = _behaviour == MockAuthBehaviour.AcceptByDefault ? new Option<UserDto>(TestData.user) : new Option<UserDto>()
            };
        }

        public string GenerateToken(int userId)
        {
            return "";
        }
    }
}
