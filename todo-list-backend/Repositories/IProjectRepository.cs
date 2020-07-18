using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectRecord> List(Func<ProjectRecord, bool> predicate, int skip, int take);
        Option<ProjectRecord> Find(Func<ProjectRecord, bool> predicate);
        ProjectRecord Save(ProjectRecord record);
    }
}
