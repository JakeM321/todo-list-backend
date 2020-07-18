using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Models.User;
using todo_list_backend.Repositories;
using todo_list_backend.Types;

namespace todo_list_backend.Services.Project
{
    public class ProjectService : IProjectService
    {
        private IProjectRepository _projectRepository;
        private IProjectMembershipRepository _projectMembershipRepository;
        private IProjectTaskRepository _projectTaskRepository;
        private IUserService _userService;

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectMembershipRepository projectMembershipRepository,
            IProjectTaskRepository projectTaskRepository,
            IUserService userService)
        {
            _projectRepository = projectRepository;
            _projectMembershipRepository = projectMembershipRepository;
            _projectTaskRepository = projectTaskRepository;
            _userService = userService;
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
                .Select(record => new ProjectDto(record, record.UserId == userId, false))
                .ToArray();
        }

        public Option<ProjectDto> GetInfo(int userId, int projectId)
        {
            return _projectRepository
                .Find(record => record.Id == projectId)
                .Get(
                    record => new Option<ProjectDto>(new ProjectDto(record, record.UserId == userId, false)),
                    () => new Option<ProjectDto>()
                );
        }

        private ProjectTaskDto[] GetDtos(IEnumerable<Tuple<ProjectTaskRecord, UserRecord>> records)
        {
            return records
                .Select(pair => new ProjectTaskDto(
                    pair.Item1,
                    pair.Item2,
                    false
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

        public bool VerifyMembership(int userId, int projectId)
        {
            return _projectMembershipRepository
                .Find(m => m.UserId == userId && m.ProjectId == projectId)
                .Get(m => true, () => false);
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

        public ProjectMemberDto[] ListMembers(int projectId)
        {
            return _projectMembershipRepository
                .ListWithUsers(project => project.Id == projectId)
                .Select(pair => new ProjectMemberDto {
                    DisplayName = pair.Item2.DisplayName,
                    Email = pair.Item2.Email
                })
                .ToArray();
        }
    }
}
