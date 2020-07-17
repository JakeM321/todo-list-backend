using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Services;
using todo_list_backend.Types;

namespace todo_list_backend.Handlers
{
    public class AppAuthenticationOptions : AuthenticationSchemeOptions
    {
    }

    public class AppAuthenticationHandler : AuthenticationHandler<AppAuthenticationOptions>
    {
        public AppAuthenticationHandler(
            IOptionsMonitor<AppAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        private Option<UserDto> GetAuthenticatedUser(HttpContext context)
        {
            return context.Items.ContainsKey("user") ? new Option<UserDto>((UserDto)context.Items["user"]) : new Option<UserDto>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = GetAuthenticatedUser(Context).Get(user =>
            {
                var claims = new[] { new Claim(ClaimTypes.Name, user.DisplayName) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new System.Security.Principal.GenericPrincipal(identity, null);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }, () =>
            {
                return AuthenticateResult.Fail("unauthorized");
            });

            return Task.FromResult(result);
        }
    }
}
