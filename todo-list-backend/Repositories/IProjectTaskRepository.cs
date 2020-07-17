using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;

namespace todo_list_backend.Repositories
{
    public interface IProjectTaskRepository
    {
        ProjectTaskRecord Save(ProjectTaskRecord record);
        IEnumerable<ProjectTaskRecord> List(Func<ProjectTaskRecord, bool> predicate, int skip, int take);
    }
}
