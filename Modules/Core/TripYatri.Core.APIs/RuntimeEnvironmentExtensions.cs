using Microsoft.Extensions.Hosting;
using TripYatri.Core.Base;

namespace TripYatri.Core.API
{
    public static class RuntimeEnvironmentExtensions
    {
        public static RuntimeEnvironment GetCurrentRuntimeEnvironment(this IHostEnvironment hostEnvironment)
        {
            return RuntimeEnvironment.FromName(hostEnvironment.EnvironmentName);
        }
    }
}
