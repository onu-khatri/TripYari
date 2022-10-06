using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
