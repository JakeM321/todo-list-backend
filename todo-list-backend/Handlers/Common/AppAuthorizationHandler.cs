using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;

namespace todo_list_backend.Handlers.Common
{
    public abstract class AppAuthorizationHandler<TRequirement> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IHttpContextAccessor _httpContextAccessor;

        public AppAuthorizationHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _scopeFactory = scopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        protected abstract Task<bool> ValidateRequestAsync(IServiceProvider serviceProvider, UserDto user, HttpRequest request);

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var httpContext = _httpContextAccessor.HttpContext;

                var user = (UserDto)httpContext.Items["user"];
                var valid = await ValidateRequestAsync(scope.ServiceProvider, user, httpContext.Request);

                if (valid)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
        }
    }
}
