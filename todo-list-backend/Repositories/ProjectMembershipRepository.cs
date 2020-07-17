using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public class ProjectMembershipRepository : IProjectMembershipRepository
    {
        private AppDbContext _db;
        private ILogger _logger;

        public ProjectMembershipRepository(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger("Project Membership Respository");
        }

        public Option<ProjectMembershipRecord> Find(Func<ProjectMembershipRecord, bool> predicate)
        {
            var search = _db.ProjectMemberships.Where(predicate);
            return search.Any() ? new Option<ProjectMembershipRecord>(search.First()) : new Option<ProjectMembershipRecord>();
        }

        public void Save(ProjectMembershipRecord record)
        {
            _db.ProjectMemberships.Add(record);
            _db.SaveChanges();
        }

        public IEnumerable<ProjectMembershipRecord> List(Func<ProjectMembershipRecord, bool> predicate)
        {
            return _db.ProjectMemberships.Where(predicate);
        }
    }
}
