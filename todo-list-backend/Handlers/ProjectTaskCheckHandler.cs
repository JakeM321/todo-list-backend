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
    public class ProjectTaskCheckHandler : RequestQueryAuthorizationHandler<TaskBelongsToProjectRequirement>
    {
        public ProjectTaskCheckHandler(IServiceScopeFactory factory, IHttpContextAccessor httpContextAccessor)
            : base(factory, httpContextAccessor) { }

        protected override bool ValidateRequestQuery(IServiceProvider serviceProvider, UserDto user, IQueryCollection query)
        {
            return GetQueryParam<int>(query, "projectId", Convert.ToInt32).Get(projectId =>
            {
                return GetQueryParam<int>(query, "projectTaskId", Convert.ToInt32).Get(projectTaskId =>
                {
                    var projectTaskService = serviceProvider.GetRequiredService<IProjectTaskService>();
                    return projectTaskService.VerifyTaskBelongsToProject(projectId, projectTaskId);
                }, () => false);
            }, () => false);
        }
    }

    public class TaskBelongsToProjectRequirement: IAuthorizationRequirement { }
}
