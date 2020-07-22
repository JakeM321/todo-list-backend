using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Task;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;
using todo_list_backend.Repositories;

namespace todo_list_backend.Services.Project
{
    public class ProjectTaskService : IProjectTaskService
    {
        private IProjectTaskRepository _projectTaskRepository;
        private IUserService _userService;

        public ProjectTaskService(
            IProjectTaskRepository projectTaskRepository,
            IUserService userService
        ) {
            _projectTaskRepository = projectTaskRepository;
            _userService = userService;
        }

        private ProjectTaskDto[] GetDtos(IEnumerable<Tuple<ProjectTaskRecord, UserRecord>> records)
        {
            return records
                .Select(pair => new ProjectTaskDto(
                    pair.Item1,
                    pair.Item2
                 ))
                .ToArray();
        }

        public ProjectTaskDto[] ListProjectTasks(int projectId, int skip, int take)
        {
            return GetDtos(
                _projectTaskRepository.List(task => task.ProjectId == projectId, skip, take)
            );
        }

        public ProjectTaskDto[] ListUserTasks(int userId, int skip, int take)
        {
            return GetDtos(
                _projectTaskRepository.List(task => task.UserId == userId, skip, take)
            );
        }

        public CreateProjectTaskResultDto CreateProjectTask(int projectId, CreateProjectTaskDto dto)
        {
            return _userService.FindByEmail(dto.AssignedTo.Email).Get(assignee =>
            {
                var record = _projectTaskRepository.Save(new ProjectTaskRecord
                {
                    ProjectId = projectId,
                    UserId = assignee.Id,
                    Label = dto.Label,
                    Description = dto.Description
                });

                return new CreateProjectTaskResultDto { ValidUser = true, Id = record.Id };
            }, () => new CreateProjectTaskResultDto { ValidUser = false });

        }

        public void SetCompletion(int userId, int projectTaskId, bool completed)
        {
            _projectTaskRepository.Update(
                r => r.Id == projectTaskId && r.UserId == userId,
                record => {
                    record.Completed = completed;
                    return record;
                }
            );
        }

        public bool VerifyTaskBelongsToProject(int projectId, int projectTaskId)
        {
            return _projectTaskRepository
                .Find(r => r.Id == projectTaskId)
                .Get(
                    record => record.ProjectId == projectId,
                    () => false
                );
        }
    }
}
