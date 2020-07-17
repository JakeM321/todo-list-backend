using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Handlers.Common;
using todo_list_backend.Models.User;
using todo_list_backend.Services.Project;

namespace todo_list_backend.Handlers
{
    public class ProjectMembershipHandler : RequestQueryAuthorizationHandler<UserHasProjectMembershipRequirement>
    {
        public ProjectMembershipHandler(IServiceScopeFactory factory, IHttpContextAccessor httpContextAccessor)
            : base(factory, httpContextAccessor) { }

        protected override bool ValidateRequestQuery(IServiceProvider serviceProvider, UserDto user, IQueryCollection query)
        {
            return GetQueryParam<int>(query, "projectId", Convert.ToInt32).Get(projectId =>
            {
                var projectService = serviceProvider.GetRequiredService<IProjectService>();
                return projectService.VerifyMembership(user.Id, projectId);
            }, () => false);
        }
    }

    public class UserHasProjectMembershipRequirement: IAuthorizationRequirement { }
}
