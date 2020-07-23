using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Membership;
using todo_list_backend.Models.Project.Record;
using todo_list_backend.Repositories;

namespace todo_list_backend.Services.Project
{
    public class ProjectMembershipService : IProjectMembershipService
    {
        private IProjectMembershipRepository _projectMembershipRepository;

        public ProjectMembershipService(IProjectMembershipRepository projectMembershipRepository)
        {
            _projectMembershipRepository = projectMembershipRepository;
        }

        public bool VerifyMembership(int userId, int projectId)
        {
            return _projectMembershipRepository
                .Find(m => m.UserId == userId && m.ProjectId == projectId)
                .Get(m => true, () => false);
        }

        public ProjectMemberDto[] ListMembers(int projectId)
        {
            return _projectMembershipRepository
                .ListWithUsers(membership => membership.ProjectId == projectId)
                .Select(pair => new ProjectMemberDto
                {
                    UserId = pair.Item2.Id,
                    DisplayName = pair.Item2.DisplayName,
                    Email = pair.Item2.Email
                })
                .ToArray();
        }

        public void AddMember(int userId, int projectId)
        {
            _projectMembershipRepository.Save(new ProjectMembershipRecord { UserId = userId, ProjectId = projectId, IsFavourite = false });
        }
    }
}
