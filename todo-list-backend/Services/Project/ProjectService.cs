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

        public ProjectService(
            IProjectRepository projectRepository,
            IProjectMembershipRepository projectMembershipRepository
            )
        {
            _projectRepository = projectRepository;
            _projectMembershipRepository = projectMembershipRepository;
            
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
            var projectsForUser = memberships.ToDictionary(m => m.ProjectId, m => m);

            return _projectRepository
                .List(project => projectsForUser.ContainsKey(project.UserId), skip, take)
                .Select(record => new ProjectDto(record, record.UserId == userId, projectsForUser[record.Id].IsFavourite ))
                .ToArray();
        }

        public Option<ProjectDto> GetInfo(int userId, int projectId)
        {
            return _projectMembershipRepository
                .Find(membership => membership.UserId == userId && membership.ProjectId == projectId)
                .Get(membership =>
                {
                    return _projectRepository
                        .Find(record => record.Id == projectId)
                        .Get(
                            record => new Option<ProjectDto>(new ProjectDto(record, record.UserId == userId, membership.IsFavourite)),
                            () => new Option<ProjectDto>()
                        );
                }, () => new Option<ProjectDto>());
        }

        public void SetFavourite(int userId, int projectId, bool favourite)
        {
            _projectMembershipRepository.Update(
                r => r.Id == projectId && r.UserId == userId,
                record => {
                    record.IsFavourite = favourite;
                    return record;
                }
            );
        }
    }
}
