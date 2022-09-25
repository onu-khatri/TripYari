using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Data.DbMigrations.Services.DbMigrator
{
    /// <summary>
    /// Inspects and migrates all configured databases as a background task using the IHostedProvider mechanism
    /// </summary>
    public class DbMigratorBackgroundProvider : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DbMigratorBackgroundProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Create a new scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            var dbMigratorProvider = scope.ServiceProvider.GetRequiredService<IDbMigratorProvider>();
            var dataOptions = scope.ServiceProvider.GetRequiredService<IOptions<DataOptions>>();

            logger.LogInfo("Starting database migrations...");

            var migrationExceptions = new List<Exception>();
            foreach (var dbOptions in dataOptions.Value.Connections)
            {
                try
                {                    
                    // Skip databases with disabled migrations
                    if (!dbOptions.Value.EnableMigrations)
                    {
                        logger.LogInfo($"Disabled migrations database {dbOptions.Key}. Skipping...");
                        continue;
                    }

                    // Skip read-only databases
                    if (dbOptions.Key.EndsWith("Reader"))
                    {
                        logger.LogInfo($"Read only database {dbOptions.Key}. Skipping...");
                        continue;
                    }

                    logger.LogInfo($"Starting database migration of {dbOptions.Key}");
                    await dbMigratorProvider.Migrate(dbOptions.Value.databaseType);
                    logger.LogInfo($"Finished database migration of {dbOptions.Key}");
                }
                catch (Exception e)
                {
                    logger.LogError($"Failed database migration of {dbOptions.Key}", e);
                    migrationExceptions.Add(e);
                }
            }

            if (migrationExceptions.Any())
                throw new AggregateException($"Failed to migrate {migrationExceptions.Count} database(s)", migrationExceptions);

            logger.LogInfo("Database migrations complete");
        }
    }
}