using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TripYari.Core.RuntimeContext
{
    public class RuntimeContextProvider : IRuntimeContextProvider
    {
        private int _logicalClock;
        private string RequestId { get; set; } = "_";

        public DateTimeOffset RequestStartTime { get; }
        public DateTimeOffset RequestMaxEndTime { get; }

        public TimeSpan RequestRemainingTime
        {
            get
            {
                if (DateTimeOffset.Now > RequestMaxEndTime) return TimeSpan.Zero;
                return RequestMaxEndTime - DateTimeOffset.Now;
            }
        }

        public string Team { get; set; }
        public string Application { get; set; }
        public string BuildVersion { get; set; }
        public RuntimeEnvironment CurrentEnvironment { get; }

        /// <summary>
        /// Denotes if the current request is in Debug mode
        /// </summary>
        public bool IsDebug { get; set; }


        public string OAuthOwnerId { get; set; } = string.Empty;
        public string OAuthClientId { get; set; } = string.Empty;
        public string AccountDid { get; set; } = string.Empty;
        public string UserDid { get; set; } = string.Empty;
        public string ServerName { get; set; } = Environment.MachineName;

        public readonly IDictionary<string, object> CustomProperties = new ConcurrentDictionary<string, object>();

        public RuntimeContextProvider()
            : this(RuntimeEnvironment.development)
        {
        }

        public RuntimeContextProvider(RuntimeEnvironment currentEnvironment)
        {
            Team = Environment.GetEnvironmentVariable("Team")
                   ?? Environment.GetEnvironmentVariable("TEAM")
                   ?? Environment.GetEnvironmentVariable("team")
                   ?? "UnknownTeam";
            Application = Environment.GetEnvironmentVariable("Application")
                          ?? Environment.GetEnvironmentVariable("APPLICATION")
                          ?? Environment.GetEnvironmentVariable("application")
                          ?? "UnknownApp";
            BuildVersion = Environment.GetEnvironmentVariable("BuildVersion")
                          ?? Environment.GetEnvironmentVariable("BUILD_VERSION")
                          ?? Environment.GetEnvironmentVariable("build_version")
                          ?? "UnknownVersion";
            CurrentEnvironment = currentEnvironment;
            RequestStartTime = DateTimeOffset.Now;
            RequestMaxEndTime = RequestStartTime.Add(TimeSpan.FromSeconds(15));
        }

        public RuntimeContextProvider(RuntimeEnvironment currentEnvironment, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var contextConfiguration = serviceProvider.GetService<RuntimeContextSettings>();

            Team = Environment.GetEnvironmentVariable("Team")
                   ?? Environment.GetEnvironmentVariable("TEAM")
                   ?? Environment.GetEnvironmentVariable("team")
                   ?? configuration?.GetValue<string>("Team")
                   ?? configuration?.GetValue<string>("team")
                   ?? contextConfiguration?.Team;
            Application = Environment.GetEnvironmentVariable("Application")
                          ?? Environment.GetEnvironmentVariable("APPLICATION")
                          ?? Environment.GetEnvironmentVariable("application")
                          ?? configuration?.GetValue<string>("Application")
                          ?? configuration?.GetValue<string>("application")
                          ?? contextConfiguration?.Application;
            BuildVersion = Environment.GetEnvironmentVariable("BuildVersion")
                          ?? Environment.GetEnvironmentVariable("BUILD_VERSION")
                          ?? Environment.GetEnvironmentVariable("build_version")
                          ?? configuration?.GetValue<string>("BuildVersion")
                          ?? configuration?.GetValue<string>("build_version")
                          ?? contextConfiguration?.BuildVersion;

            CurrentEnvironment = currentEnvironment;
            RequestStartTime = DateTimeOffset.Now;

            if (contextConfiguration != null)
            {
                RequestMaxEndTime = RequestStartTime.Add(contextConfiguration.ContextLifeSpan);
            }
            else
            {
                RequestMaxEndTime = RequestStartTime.Add(TimeSpan.FromSeconds(15));
            }
        }

        public void SetRequestId(string requestId)
        {
            RequestId = requestId;
        }

        public string GetRuntimeContextRequestId()
        {
            return RequestId;
        }

        /// <summary>
        /// Retrieves the <code ref="RequestId">RequestId</code> with the next incremented logical clock appended at the end.
        /// Every call returns the next incremented logical clock sequence value.
        /// </summary>
        /// <returns>The current <code ref="RequestId">RequestId</code> with a sequential <code>.&lt;seq&gt;</code> at the end.</returns>
        public string GetNextRequestId()
        {
            return $"{RequestId}.{Interlocked.Increment(ref _logicalClock)}";
        }
    }
}