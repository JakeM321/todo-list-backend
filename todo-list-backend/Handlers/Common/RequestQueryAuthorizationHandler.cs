using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo_list_backend.Models.User;
using todo_list_backend.Types;

namespace todo_list_backend.Handlers.Common
{
    public abstract class RequestQueryAuthorizationHandler<TRequirement> : AppAuthorizationHandler<TRequirement>
        where TRequirement : IAuthorizationRequirement
    {
        public RequestQueryAuthorizationHandler(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpContextAccessor) : base(scopeFactory, httpContextAccessor) { }

        protected abstract bool ValidateRequestQuery(IServiceProvider serviceProvider, UserDto user, IQueryCollection query);

        protected Option<T> GetQueryParam<T>(IQueryCollection query, string key, Func<string, T> converter)
        {
            if (query.ContainsKey(key))
            {
                var value = query[key];

                try
                {
                    var converted = converter(value);
                    return new Option<T>(converted);
                } 
                catch
                {
                    return new Option<T>();
                }
            }
            else
            {
                return new Option<T>();
            }
        }

        protected override Task<bool> ValidateRequestAsync(IServiceProvider serviceProvider, UserDto user, HttpRequest request)
        {
            return Task.FromResult(ValidateRequestQuery(serviceProvider, user, request.Query));
        }

    }
}
