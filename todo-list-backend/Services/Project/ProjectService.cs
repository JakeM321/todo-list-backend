using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Repositories;

namespace todo_list_backend.Services.Project
{
    public class ProjectService : IProjectService
    {
        private IProjectRepository _projectRepository;
        private IProjectMembershipRepository _projectMembershipRepository;
        private IProjectTaskRepository _projectTaskRepository;

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectMembershipRepository projectMembershipRepository,
            IProjectTaskRepository projectTaskRepository)
        {
            _projectRepository = projectRepository;
            _projectMembershipRepository = projectMembershipRepository;
            _projectTaskRepository = projectTaskRepository;
        }

        public CreateProjectResultDto CreateProject(int userId, CreateProjectDto dto)
        {
            var record = _projectRepository.Save(new ProjectRecord {
                UserId = userId,
                Title = dto.Title,
                Colour = dto.Colour
            });

            _projectMembershipRepository.Save(new ProjectMembershipRecord { UserId = userId, ProjectId = record.Id });

            return new CreateProjectResultDto { Id = record.Id };
        }

        public ProjectDto[] ListProjects(int userId, int skip, int take)
        {
            var memberships = _projectMembershipRepository.List(membership => membership.UserId == userId);
            var projectsForUser = memberships.Select(m => m.ProjectId).ToHashSet();

            return _projectRepository.List(project => projectsForUser.Contains(project.UserId), skip, take)
                .Select(record => new ProjectDto(record))
                .ToArray();
        }

        public ProjectTaskDto[] ListProjectTasks(int projectId, int skip, int take)
        {
            return _projectTaskRepository
                .List(task => task.ProjectId == projectId, skip, take)
                .Select(record => new ProjectTaskDto(record))
                .ToArray();
        }

        public ProjectTaskDto[] ListUserTasks(int userId, int skip, int take)
        {
            return _projectTaskRepository
                .List(task => task.UserId == userId, skip, take)
                .Select(record => new ProjectTaskDto(record))
                .ToArray();
        }

        public bool VerifyMembership(int userId, int projectId)
        {
            return _projectMembershipRepository
                .Find(m => m.UserId == userId && m.ProjectId == projectId)
                .Get(m => true, () => false);
        }

        public CreateProjectTaskResultDto CreateProjectTask(int projectId, CreateProjectTaskDto dto)
        {
            var record = _projectTaskRepository.Save(new ProjectTaskRecord
            {
                ProjectId = projectId,
                UserId = dto.UserId,
                Name = dto.Name,
                Description = dto.Description
            });

            return new CreateProjectTaskResultDto { Id = record.Id };
        }
    }
}
