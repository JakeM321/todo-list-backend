using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public class ProjectTaskRepository: IProjectTaskRepository
    {
        private AppDbContext _db;
        private ILogger _logger;

        public ProjectTaskRepository(AppDbContext db, ILoggerFactory factory)
        {
            _db = db;
            _logger = factory.CreateLogger("Project Task Respository");
        }

        public ProjectTaskRecord Save(ProjectTaskRecord record) {
            var result = _db.ProjectTasks.Add(record);
            _db.SaveChanges();
            return result.Entity;
        }

        public IEnumerable<Tuple<ProjectTaskRecord, UserRecord>> List(Func<ProjectTaskRecord, bool> predicate, int skip, int take) {
            var query =
                from task in _db.ProjectTasks.Where(predicate)
                join user in _db.Users on task.UserId equals user.Id
                select new Tuple<ProjectTaskRecord, UserRecord>(task, user);

            return query.Skip(skip).Take(take);
        }

        public Option<ProjectTaskRecord> Find(Func<ProjectTaskRecord, bool> predicate)
        {
            var search = _db.ProjectTasks.Where(predicate);
            return search.Any() ? new Option<ProjectTaskRecord>(search.First()) : new Option<ProjectTaskRecord>();
        }

        public void Update(Func<ProjectTaskRecord, bool> predicate, Func<ProjectTaskRecord, ProjectTaskRecord> transformer)
        {
            var entities = _db.ProjectTasks.Where(predicate);
            var updates = entities.Select(transformer);

            _db.ProjectTasks.UpdateRange(updates);
            _db.SaveChanges();
        }
    }
}
