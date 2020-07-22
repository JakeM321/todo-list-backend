using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto;

namespace todo_list_backend.Services.Project
{
    public interface IProjectTaskService
    {
        ProjectTaskDto[] ListProjectTasks(int projectId, int skip, int take);
        ProjectTaskDto[] ListUserTasks(int userId, int skip, int take);
        CreateProjectTaskResultDto CreateProjectTask(int projectId, CreateProjectTaskDto dto);
        void SetCompletion(int userId, int projectTaskId, bool favourite);
        bool VerifyTaskBelongsToProject(int projectId, int projectTaskId);
    }
}
