using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TripYari.Core.Metrics
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleMatrics(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IMetricsProvider, ConsoleMetricsProvider>();
            return services;
        }
    }
}
