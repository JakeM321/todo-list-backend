using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Repositories
{
    public interface IProjectMembershipRepository
    {
        IEnumerable<ProjectMembershipRecord> List(Func<ProjectMembershipRecord, bool> predicate);
        IEnumerable<Tuple<ProjectMembershipRecord, UserRecord>> ListWithUsers(Func<ProjectMembershipRecord, bool> predicate);
        Option<ProjectMembershipRecord> Find(Func<ProjectMembershipRecord, bool> predicate);
        void Save(ProjectMembershipRecord record);
        void Update(Func<ProjectMembershipRecord, bool> predicate, Func<ProjectMembershipRecord, ProjectMembershipRecord> transformer);
    }
}
