using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using todo_list_backend.Models.User;
using todo_list_backend.Services;

namespace todo_list_backend.Handlers
{
    public class NotificationOwnershipHandler : DeclarativeAuthHandler<int[], NotificationsBelongToUserRequirement>
    {
        public NotificationOwnershipHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor) : base(scopeFactory, httpContextAccessor) { }

        protected override bool ValidateRequestBody(IServiceProvider serviceProvider, UserDto user, int[] body)
        {
            var notificationProviderService = serviceProvider.GetRequiredService<INotificationProviderService>();
            var allNotificationsBelongToUser = notificationProviderService.BelongsToUser(body, user.Id);

            return allNotificationsBelongToUser;
        }
    }

    public class NotificationsBelongToUserRequirement : IAuthorizationRequirement { }
}
