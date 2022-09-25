using System.Threading.Tasks;
using TripYatri.Core.API;
using TripYatri.Core.API.Auth;
using TripYatri.Core.API.Filters;
using TripYatri.Core.API.Middlewares;
using TripYatri.Core.API.Providers;
using TripYatri.Core.API.Providers.RequestAudit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;
using TripYatri.Core.Base;

namespace TripYatri.Core.API
{
    public static class ComponentProviderCollectionExtensions
    {
        public static IServiceCollection AddCdsProviders(this IServiceCollection services,
            IHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            var runtimeEnvironment = hostEnvironment.GetCurrentRuntimeEnvironment();

            services
                .AddScoped<ILogger, ConsoleLogger>()
                .AddScoped<IRequestAuditProvider, RequestAuditProvider>();

            services.AddTransient<AuthValidator>();

            services.AddSingleton<RequestAuditBufferedProvider>();

            return services.AddCdsProviders(runtimeEnvironment, configuration);
        }

        public static IServiceCollection AddCdsAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OAuthOptions>(configuration.GetSection("OAuth"));

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("OAuth", policy => policy.Requirements.Add(new OAuthRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, OAuthHandler>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.Headers["Location"] = context.RedirectUri;
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                });

            return services.AddHttpContextAccessor();
        }

        public static IApplicationBuilder UseCdsApiMiddlewares(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RuntimeContextMiddleware>()
                .UseMiddleware<RequestAuditMiddleware>()
                .UseMiddleware<RequestLoggerMiddleware>()
                .UseMiddleware<DefaultRequestJsonContentTypeMiddleware>()
                .UseMiddleware<ExceptionMiddleware>();
        }
    }
}