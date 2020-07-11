using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _db;
        private ILogger _logger;

        public UserRepository(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger("User Respository");
        }

        public Option<User> Find(Func<User, bool> predicate)
        {
            var search = _db.Users.Where(predicate);
            return search.Any() ? new Option<User>(search.First()) : new Option<User>();
        }

        public Option<User> FindById(int id)
        {
            return Find(user => user.Id == id);
        }
    }
}
