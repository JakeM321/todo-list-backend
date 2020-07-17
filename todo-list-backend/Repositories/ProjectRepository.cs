using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;

namespace todo_list_backend.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private AppDbContext _db;
        private ILogger _logger;

        public ProjectRepository(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger("Project Respository");
        }

        public IEnumerable<ProjectRecord> List(Func<ProjectRecord, bool> predicate, int skip, int take)
        {
            return _db.Projects.Where(predicate).Skip(skip).Take(take);
        }
        
        public ProjectRecord Save(ProjectRecord record)
        {
            var entry = _db.Projects.Add(record);
            _db.SaveChanges();
            return entry.Entity;
        }
    }
}
