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

namespace todo_list_backend.Handlers.Common
{
    public abstract class RequestBodyAuthortizationHandler<TRequestBody, TRequirement> : AppAuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        public RequestBodyAuthortizationHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor) : base(scopeFactory, httpContextAccessor) { }

        protected abstract bool ValidateRequestBody(IServiceProvider serviceProvider, UserDto user, TRequestBody body);

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

        protected async override Task<bool> ValidateRequestAsync(IServiceProvider serviceProvider, UserDto user, HttpRequest request)
        {
            var body = await ReadBody(request);
            return ValidateRequestBody(serviceProvider, user, body);
        }
    }
}
