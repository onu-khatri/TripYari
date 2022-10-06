using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripYari.Core.RuntimeContext
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRuntimeContext(this IServiceCollection services,
            RuntimeEnvironment runtimeEnvironment, IConfiguration configuration)
        {
            services.AddSingleton(runtimeEnvironment);
            services.AddScoped<IRuntimeContextProvider, RuntimeContextProvider>();
            return services;
        }
    }
}
