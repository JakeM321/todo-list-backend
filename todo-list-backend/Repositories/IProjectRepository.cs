using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;

namespace todo_list_backend.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<ProjectRecord> List(Func<ProjectRecord, bool> predicate, int skip, int take);
        ProjectRecord Save(ProjectRecord record);
    }
}
