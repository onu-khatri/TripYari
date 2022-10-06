using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TripYari.Core.RuntimeContext;

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
