using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto;
using todo_list_backend.Types;

namespace todo_list_backend.Services.Project
{
    public interface IProjectService
    {
        CreateProjectResultDto CreateProject(int userId, CreateProjectDto dto);
        ProjectDto[] ListProjects(int userId, int skip, int take);
        Option<ProjectDto> GetInfo(int userId, int projectId);
        ProjectTaskDto[] ListProjectTasks(int projectId, int skip, int take);
        ProjectTaskDto[] ListUserTasks(int userId, int skip, int take);
        bool VerifyMembership(int userId, int projectId);
        CreateProjectTaskResultDto CreateProjectTask(int projectId, CreateProjectTaskDto dto);
        ProjectMemberDto[] ListMembers(int projectId);
        void SetFavourite(int userId, int projectId, bool favourite);
        void SetCompletion(int userId, int projectTaskId, bool favourite);
        bool VerifyTaskBelongsToProject(int projectId, int projectTaskId);
    }
}
