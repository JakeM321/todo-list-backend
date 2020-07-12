using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Repositories;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto CreateSsoUser(string email, string displayName)
        {
            var user = _userRepository.CreateUser(new UserRecord
            {
                Email = email,
                DisplayName = displayName,
                SsoUser = true
            });

            return new UserDto(user);
        }

        public UserDto CreateUserWithPassword(string email, string displayName, string password)
        {
            var hashAndSalt = Utils.Authentication.PasswordEncryption.GenerateHashAndSalt(password);

            var user = _userRepository.CreateUser(new UserRecord
            {
                Email = email,
                DisplayName = displayName,
                SsoUser = false,
                PasswordHash = hashAndSalt.Item1,
                Salt = hashAndSalt.Item2
            });

            return new UserDto(user);
        }

        private Option<UserDto> Find(Func<UserDto, bool> predicate)
        {
            return _userRepository.Find(record => predicate(new UserDto(record))).Get(
                result => new Option<UserDto>(new UserDto(result)),
                () => new Option<UserDto>()
            );
        }

        public Option<UserDto> FindByEmail(string email)
        {
            return Find(user => user.Email == email);
        }
        public Option<UserDto> FindByEmailRegistration(string email)
        {
            return Find(user => user.Email == email && !user.SsoUser);
        }

        public Option<UserDto> FindById(int id)
        {
            return Find(user => user.Id == id);
        }

        public bool ComparePassword(int id, string password)
        {
            return _userRepository.Find(record => record.Id == id).Get(
                record => Utils.Authentication.PasswordEncryption.VerifyPassword(record.PasswordHash, record.Salt, password),
                () => false
            );
        }
    }
}
