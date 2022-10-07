using Microsoft.Extensions.DependencyInjection;

namespace TripYari.Core.Loggers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleLogger(this IServiceCollection services)
        {
            services.AddScoped<ILogger, ConsoleLogger>();
            return services;
        }
    }
}
