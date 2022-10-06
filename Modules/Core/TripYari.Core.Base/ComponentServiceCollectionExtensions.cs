using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TripYari.Core.Base.Providers.Hash;
using TripYari.Core.Base.Providers.Json;
using TripYari.Core.Base.Providers.Time;

namespace TripYari.Core.Base
{
    public static class ComponentProviderCollectionExtensions
    {
        public static IServiceCollection AddCdsProviders(this IServiceCollection services,
            IConfiguration configuration)
        {
            // per https://stackexchange.github.io/StackExchange.Redis/Timeouts
            // Dealing with Redis timeouts
            // This also helps dealing with potential thread-starvation of IO ports
            ThreadPool.SetMinThreads(200, 200);

            services.AddTransient<IJsonProvider, NewtonsoftJsonProvider>();           
            services.AddTransient<IHashProvider, HashProvider>();
            services.AddSingleton<ITimeProvider, TimeProvider>();

            return services;
        }

        public static IServiceCollection Replace<TProvider, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
            where TProvider : class
            where TImplementation : class, TProvider
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TProvider));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TProvider), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);

            return services;
        }
    }
}
