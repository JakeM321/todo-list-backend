﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.Project.Dto.Membership;

namespace todo_list_backend.Services.Project
{
    public interface IProjectMembershipService
    {
        bool VerifyMembership(int userId, int projectId);
        ProjectMemberDto[] ListMembers(int projectId);
        void AddMember(int userId, int projectId);
    }
}
