using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public interface IUserRepository
    {
        Option<User> Find(Func<User, bool> predicate);
        Option<User> FindById(int id);
    }
}
