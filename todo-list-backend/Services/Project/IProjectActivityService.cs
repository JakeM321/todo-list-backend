using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Activity;

namespace todo_list_backend.Services.Project
{
    public interface IProjectActivityService
    {
        IEnumerable<ProjectActivityDto> GetProjectActivity(int projectId, int skip, int take);
    }
}
