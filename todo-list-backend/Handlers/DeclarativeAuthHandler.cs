using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Services;

namespace todo_list_backend.Handlers
{
    public abstract class DeclarativeAuthHandler<TRequestBody, TRequirement> : AuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IHttpContextAccessor _httpContextAccessor;
        public DeclarativeAuthHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _scopeFactory = scopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<TRequestBody> ReadBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var content = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return JsonConvert.DeserializeObject<TRequestBody>(content);
            }
        }

        protected abstract bool ValidateRequestBody(IServiceProvider serviceProvider, UserDto user, TRequestBody body);

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var user = (UserDto)this._httpContextAccessor.HttpContext.Items["user"];
                var resource = await ReadBody(_httpContextAccessor.HttpContext.Request);

                var valid = ValidateRequestBody(scope.ServiceProvider, user, resource);

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
