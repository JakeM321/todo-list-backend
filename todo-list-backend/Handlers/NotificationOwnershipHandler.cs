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
    public class NotificationOwnershipHandler : AuthorizationHandler<NotificationsBelongToUserRequirement>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IHttpContextAccessor _httpContextAccessor;
        public NotificationOwnershipHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _scopeFactory = scopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<T> ReadBody<T>(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var content = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, NotificationsBelongToUserRequirement requirement)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var notificationProviderService = scope.ServiceProvider.GetRequiredService<INotificationProviderService>();

                var user = (UserDto)this._httpContextAccessor.HttpContext.Items["user"];

                var resource = await ReadBody<int[]>(_httpContextAccessor.HttpContext.Request);

                var allBelongToUser = notificationProviderService.BelongsToUser(resource, user.Id);

                if (allBelongToUser)
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

    public class NotificationsBelongToUserRequirement : IAuthorizationRequirement { }
}
