using Amazon.CloudWatch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripYari.Core.Aws.CloudWatchMetrics;
using TripYari.Core.Aws.Queue;
using TripYari.Core.Aws.S3;
using TripYari.Core.Aws.Sns;
using TripYari.Core.Loggers;
using TripYari.Core.Metrics;
using TripYari.Core.RuntimeContext;

namespace TripYari.Core.Aws
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConsoleLogger(this IServiceCollection services, RuntimeEnvironment runtimeEnvironment, IConfiguration configuration)
        {
            services.Configure<CloudWatchMetricsOptions>(configuration.GetSection("CloudWatchMetrics"));
            services.AddTransient<IAmazonCloudWatch, AmazonCloudWatchClient>();
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
                    .AddSingleton<IS3Provider, AwsS3Provider>();
            }
            return services;
        }
    }
}
