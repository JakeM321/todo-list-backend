using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
using Microsoft.Extensions.Primitives;
using todo_list_backend.Repositories;
using todo_list_backend.Services;
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
            services.AddDbContext<AppDbContext>(options => options.UseSqlite(SqliteSetup.ConnectionString));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthTokenService, AuthTokenService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
        }

        private Option<string> GetHeader(HttpContext context, string key)
        {
            if (context.Request.Headers.ContainsKey(key))
            {
                var data = context.Request.Headers[key];
                return data.Count > 0 ? new Option<string>(data[0]) : new Option<string>();
            }
            else
            {
                return new Option<string>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });

                SqliteSetup.RunSetup();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseWhen(context => !context.Request.Path.Value.Contains("email-login"), builder => builder.Use(async (context, next) =>
            {
                await GetHeader(context, "token").Get(async token =>
                {
                    var authTokenService = context.RequestServices.GetRequiredService<IAuthTokenService>();
                    var authAttempt = authTokenService.AuthenticateToken(token);

                    if (authAttempt.Accepted)
                    {
                        var newToken = authAttempt.Token;
                        context.Response.Headers.Add("token", newToken);
                        context.Items.Add("user", authAttempt.User);

                        await next.Invoke();
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("unauthorized");
                    }
                }, async () =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsync("unauthorized");
                });
            }));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
