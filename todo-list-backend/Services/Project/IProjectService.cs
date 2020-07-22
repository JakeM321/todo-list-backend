using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Project;
using todo_list_backend.Types;

namespace todo_list_backend.Services.Project
{
    public interface IProjectService
    {
        CreateProjectResultDto CreateProject(int userId, CreateProjectDto dto);
        ProjectDto[] ListProjects(int userId, int skip, int take);
        Option<ProjectDto> GetInfo(int userId, int projectId);
        void SetFavourite(int userId, int projectId, bool favourite);
    }
}
