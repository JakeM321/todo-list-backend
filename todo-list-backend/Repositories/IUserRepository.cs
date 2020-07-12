using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public interface IUserRepository
    {
        Option<UserRecord> Find(Func<UserRecord, bool> predicate);
        UserRecord CreateUser(UserRecord user);
    }
}
