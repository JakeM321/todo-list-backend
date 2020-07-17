using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;

namespace todo_list_backend.Handlers.Common
{
    public abstract class RequestQueryAuthorizationHandler<TRequirement> : AppAuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        public RequestQueryAuthorizationHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor) : base(scopeFactory, httpContextAccessor) { }

        protected abstract bool ValidateRequestQuery(IServiceProvider serviceProvider, UserDto user, IQueryCollection query);

        protected override Task<bool> ValidateRequestAsync(IServiceProvider serviceProvider, UserDto user, HttpRequest request)
        {
            return Task.FromResult(ValidateRequestQuery(serviceProvider, user, request.Query));
        }

    }
}
