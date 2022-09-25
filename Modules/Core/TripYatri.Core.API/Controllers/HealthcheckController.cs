using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using TripYatri.Core.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Metrics;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthcheckController : ControllerBase
    {
        private static bool _warmedUp = false;
        private static readonly object WarmUpSync = new object();

        private IServiceProvider ServiceProvider { get; }
        private IMetricsProvider MetricsProvider { get; }
        private ILogger Logger { get; }

        public HealthcheckController(IServiceProvider serviceProvider, IMetricsProvider metricsProvider, ILogger logger)
        {
            ServiceProvider = serviceProvider;
            MetricsProvider = metricsProvider;
            Logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            WarmUp();

            return Ok();
        }

        private void WarmUp()
        {
            if (_warmedUp) return;
            lock (WarmUpSync)
            {
                using (MetricsProvider.BeginTiming(this))
                {
                    Logger.LogInfo("Application is warming up...");

                    EagerLoadAllAssemblies();

                    Logger.LogInfo("Preloading other services...");
                    _ = ServiceProvider.GetService<OAuthHandler>();

                    _warmedUp = true;
                    Logger.LogInfo("Application warmed up");
                }
            }
        }

        /// <summary>
        /// Eager loads all referenced assemblies.
        /// </summary>
        /// <remarks>
        /// Source: https://dotnetstories.com/blog/Dynamically-pre-load-assemblies-in-a-ASPNET-Core-or-any-C-project-en-7155735300
        /// </remarks>
        /// <param name="includeFramework"></param>
        private void EagerLoadAllAssemblies(bool includeFramework = false)
        {
            Logger.LogInfo("Eager loading all referenced assemblies...");

            // Storage to ensure not loading the same assembly twice and optimize calls to GetAssemblies()
            var loaded = new ConcurrentDictionary<string, bool>();

            // Filter to avoid loading all the .net framework
            bool ShouldLoad(string assemblyName)
            {
                return (includeFramework || NotNetFramework(assemblyName))
                    && !loaded.ContainsKey(assemblyName);
            }

            bool NotNetFramework(string assemblyName)
            {
                return !assemblyName.StartsWith("Microsoft.")
                    && !assemblyName.StartsWith("System.")
                    && !assemblyName.StartsWith("Newtonsoft.")
                    && assemblyName != "netstandard";
            }

            void LoadReferencedAssembly(Assembly assembly)
            {
                // Check all referenced assemblies of the specified assembly
                foreach (var assemblyName in assembly.GetReferencedAssemblies().Where(a => ShouldLoad(a.FullName)))
                {
                    // Load the assembly and load its dependencies
                    LoadReferencedAssembly(Assembly.Load(assemblyName)); // AppDomain.CurrentDomain.Load(name)
                    loaded.TryAdd(assemblyName.FullName, true);
                    Logger.LogInfo($">> Loading referenced assembly => {assemblyName.FullName}");
                }
            }

            // Populate already loaded assemblies
            Logger.LogInfo($">> Already loaded assemblies:");
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies().Where(a => ShouldLoad(a.FullName)))
            {
                loaded.TryAdd(a.FullName, true);
                Logger.LogInfo($">>>> {a.FullName}");
            }
            var alreadyLoaded = loaded.Keys.Count();
            var sw = new System.Diagnostics.Stopwatch();

            // Loop on loaded assemblies to load dependencies (it includes Startup assembly so should load all the dependency tree) 
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => NotNetFramework(a.FullName)))
                LoadReferencedAssembly(assembly);

            // Debug
            Logger.LogInfo($"\n>> Assemblies loaded after scan ({(loaded.Keys.Count - alreadyLoaded)} assemblies in {sw.ElapsedMilliseconds} ms):");
            foreach (var a in loaded.Keys.OrderBy(k => k))
                Logger.LogInfo($">>>> {a}");
        }
    }
}
