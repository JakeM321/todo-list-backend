using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
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

        public Option<UserRecord> Find(Func<UserRecord, bool> predicate)
        {
            var search = _db.Users.Where(predicate);
            return search.Any() ? new Option<UserRecord>(search.First()) : new Option<UserRecord>();
        }

        public Option<UserRecord> FindById(int id)
        {
            return Find(user => user.Id == id);
        }

        public Option<UserRecord> FindByEmail(string email)
        {
            return Find(user => user.Email == email);
        }

        public UserRecord CreateUser(UserRecord user)
        {
            var result = _db.Users.Add(user);
            _db.SaveChanges();
            return result.Entity;
        }
    }
}
