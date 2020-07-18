using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public interface IProjectTaskRepository
    {
        ProjectTaskRecord Save(ProjectTaskRecord record);
        IEnumerable<Tuple<ProjectTaskRecord, UserRecord>> List(Func<ProjectTaskRecord, bool> predicate, int skip, int take);
        Option<ProjectTaskRecord> Find(Func<ProjectTaskRecord, bool> predicate);
        void Update(Func<ProjectTaskRecord, bool> predicate, Func<ProjectTaskRecord, ProjectTaskRecord> transformer);
    }
}
