using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using todo_list_backend.Handlers;
using todo_list_backend.Repositories;
using todo_list_backend.Services;
using todo_list_backend.Services.Notifications;
using todo_list_backend.SignalR;
using todo_list_backend.Types;

namespace todo_list_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(SqliteSetup.ConnectionString));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthTokenService, AuthTokenService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IOAuthProviderService, OAuthProviderService>();
            services.AddTransient<IOAuthHandlerService, OAuthHandlerService>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddSingleton<INotificationHubManager, NotificationHubManager>();
            services.AddTransient<INotificationSenderService, NotificationSenderService>();
            services.AddTransient<INotificationProviderService, NotificationProviderService>();

            services.AddAuthentication("Basic").AddScheme<AppAuthenticationOptions, AppAuthenticationHandler>("Basic", null);
        }

        private Option<string> GetData(HttpContext context, string key, Func<HttpContext, IEnumerable<KeyValuePair<string, StringValues>>> selector)
        {
            var collection = selector.Invoke(context).ToDictionary(x => x.Key, x => x.Value);
            
            if (collection.ContainsKey(key))
            {
                var data = collection[key];
                return data.Count > 0 ? new Option<string>(data[0]) : new Option<string>();
            }
            else
            {
                return new Option<string>();
            }
        }

        private Option<string> GetHeader(HttpContext context, string key)
        {
            return GetData(context, key, ctx => ctx.Request.Headers);
        }

        private Option<string> GetQueryParam(HttpContext context, string key)
        {
            return GetData(context, key, ctx => ctx.Request.Query);
        }

        private Option<string> GetToken(HttpContext context)
        {
            var key = "token";

            return GetHeader(context, key).Get(
                token => new Option<string>(token),
                () => GetQueryParam(context, key)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                SqliteSetup.RunSetup();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });

            app.Use(async (context, next) =>
            {
                await GetToken(context).Get(async token =>
                {
                    if (!String.IsNullOrEmpty(token))
                    {
                        var authTokenService = context.RequestServices.GetRequiredService<IAuthTokenService>();
                        var authAttempt = authTokenService.AuthenticateToken(token);

                        if (authAttempt.Accepted)
                        {
                            var newToken = authAttempt.Token;
                            context.Response.Headers.Add("token", newToken);
                            context.Items.Add("user", authAttempt.User.Get(user => user, () => null));
                        }
                    }

                    await next.Invoke();

                }, async () => {
                    await next.Invoke();
                });
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification");
            });

        }
    }
}
