using Amazon.CloudWatch;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Metrics;
using TripYatri.Core.Base.Providers.ProxyContactInformation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using TripYatri.Core.Base.Providers.Hash;
using TripYatri.Core.Base.Providers.Json;
using TripYatri.Core.Base.Providers.Queue;
using TripYatri.Core.Base.Providers.Sns;
using TripYatri.Core.Base.Providers.Time;
using TripYatri.Core.Base.Providers.S3;

namespace TripYatri.Core.Base
{
    public static class ComponentProviderCollectionExtensions
    {
        public static IServiceCollection AddCdsProviders(this IServiceCollection services,
            RuntimeEnvironment runtimeEnvironment, IConfiguration configuration)
        {
            // per https://stackexchange.github.io/StackExchange.Redis/Timeouts
            // Dealing with Redis timeouts
            // This also helps dealing with potential thread-starvation of IO ports
            ThreadPool.SetMinThreads(200, 200);

            services.Configure<CloudWatchMetricsOptions>(configuration.GetSection("CloudWatchMetrics"));
            services.Configure<ProxyContactOptions>(configuration.GetSection("ProxyContact"));

            services.AddTransient<IAmazonCloudWatch, AmazonCloudWatchClient>();
            services.AddTransient<IJsonProvider, NewtonsoftJsonProvider>();           
            services.AddTransient<IHashProvider, HashProvider>();
            services.AddScoped<IProxyContactInformationProvider, ShuffledDictionaryProxyContactInformationProvider>();
            services.AddScoped<RuntimeContext>();           
            services.AddSingleton(runtimeEnvironment);
            services.AddSingleton<ITimeProvider, TimeProvider>();

            if (runtimeEnvironment.IsDevelopment())
            {
                services
                    .AddScoped<IQueueProvider, ConsoleQueueProvider>()
                    .AddScoped<IMetricsProvider, ConsoleMetricsProvider>()
                    .AddScoped<ISnsProvider, ConsoleSnsProvider>()
                    .AddScoped<IS3Provider, LocalS3Provider>();
            }
            else
            {
                services
                    .AddScoped<ISnsProvider, SnsProvider>();
                services
                    .AddSingleton<IQueueProvider, SqsQueueProvider>()
                    .AddSingleton<IMetricsProvider, CloudWatchMetricsProvider>()
                    .AddSingleton<IS3Provider,AwsS3Provider>();            
            }


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