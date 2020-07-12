using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Services
{
    public interface IUserService
    {
        Option<UserDto> FindById(int id);
        Option<UserDto> FindByEmail(string email);
        Option<UserDto> FindByEmailRegistration(string email);
        UserDto CreateUserWithPassword(string email, string displayName, string password);
        UserDto CreateSsoUser(string email, string displayName);
        bool ComparePassword(int userId, string password);
    }
}
