using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TripYatri.Core.Data.DbMigrations.Repositories;
using DbUp;
using MySqlConnector;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Data.DbMigrations.Services.DbMigrator
{
    public class DbMigratorProvider : IDbMigratorProvider
    {
        private readonly IDataContextFactory _dataContextFactory;
        private readonly ILogger _logger;
        private readonly RuntimeContext _runtimeContext;

        public DbMigratorProvider(IDataContextFactory dataContextFactory, ILogger logger, RuntimeContext runtimeContext)
        {
            _dataContextFactory = dataContextFactory;
            _logger = logger;
            _runtimeContext = runtimeContext;
        }

        public async Task Migrate(Database db, IDictionary<string, string> migrationVariables = null)
        {
            try
            {
                var migrationId = Guid.NewGuid();
                _logger.LogInfo($"Migrating database {db} with id {migrationId}...");

                using var repositoryDataContext = _dataContextFactory.CreateRepositoryDataContext();
                var dbMigrationsRepository = repositoryDataContext.GetRepository<DbMigrationsRepository>();
                if (!await LockMigration(db, dbMigrationsRepository, migrationId))
                {
                    _logger.LogInfo(
                        $"Lock for database {db} migration with id {migrationId} could not be acquired");
                    return;
                }

                _logger.LogInfo($"Lock for database {db} migration with id {migrationId} acquired");
                _logger.LogInfo("Starting background timer for database migration...");
                await using var migrationCheckInTimer = new Timer(
                    async _ =>
                    {
                        try
                        {
                            _logger.LogInfo($"Checking in on migration {migrationId}...");
                            await dbMigrationsRepository.CheckIn(db, migrationId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Failed to check in migration {migrationId}", ex);
                        }
                    },
                    null,
                    TimeSpan.FromMinutes(1),
                    TimeSpan.FromMinutes(1));

                try
                {
                    _logger.LogInfo("Discovering TripYatri.Core.* assemblies");
                    var cdsAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                        .Where(assembly => assembly.FullName.StartsWith("TripYatri.Core"))
                        .ToArray();
                    _logger.LogInfo($"Discovered {cdsAssemblies.Length} assemblies");

                    using var sqlDataContext = _dataContextFactory.CreateSQLDataContext();
                    var connectionString = sqlDataContext.GetConnectionString(db);

                    _logger.LogInfo($"Ensuring database created");
                    switch (db.DatabaseProviderType)
                    {
                        case DatabaseProvider.SqlServer:
                            EnsureDatabase.For.SqlDatabase(connectionString);
                            break;
                        case DatabaseProvider.MySql:
                        default:
                            // allow MySql User Variables
                            connectionString += ";Allow User Variables=True";
                            EnsureDatabase.For.MySqlDatabase(connectionString);
                            break;
                    }

                    _logger.LogInfo("Building upgrader...");
                    var upgraderBuilder = db.DatabaseProviderType switch
                    {
                        DatabaseProvider.SqlServer => DeployChanges.To.SqlDatabase(connectionString),
                        _ => DeployChanges.To.MySqlDatabase(connectionString)
                    };

                    upgraderBuilder = upgraderBuilder.WithScriptsEmbeddedInAssembly(
                            Assembly.GetExecutingAssembly(),
                            s => s.Contains($".DbMigrations.", StringComparison.InvariantCultureIgnoreCase)
                        )
                        .WithScriptsEmbeddedInAssemblies(
                            cdsAssemblies,
                            s => s.Contains($".DbMigrations.{db}.", StringComparison.InvariantCultureIgnoreCase)
                        );

                    // An attempt to load IScript providers, but ended up duplicating the embedded sql files
                    // foreach (var cdsAssembly in cdsAssemblies)
                    //     upgraderBuilder = upgraderBuilder
                    //         .WithScriptsAndCodeEmbeddedInAssembly(
                    //             cdsAssembly,
                    //             s => s.Contains(".DbMigrations."));

                    var upgrader = upgraderBuilder
                        .WithVariable("Environment", _runtimeContext.CurrentEnvironment.ToString())
                        .WithVariables(migrationVariables ?? new Dictionary<string, string>())
                        .WithExecutionTimeout(TimeSpan.FromMinutes(15))
                        .LogTo(new DbMigratorUpgradeLog(_logger))
                        .LogScriptOutput()
                        .Build();

                    _logger.LogInfo($"Upgrader built... starting now");
                    var result = upgrader.PerformUpgrade();

                    if (!result.Successful)
                    {
                        _logger.LogError(
                            $"Failed to upgrade database {db}: {result.ErrorScript?.Name ?? "???"} failed",
                            result.Error);
                        throw new DbMigrationException(
                            $"Failed to upgrade database {db}: {result.ErrorScript?.Name ?? "???"} failed",
                            result.Error);
                    }

                    _logger.LogInfo($"Database {db} successfully upgraded");
                }
                finally
                {
                    migrationCheckInTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    try
                    {
                        await dbMigrationsRepository.FinishMigration(db, migrationId);
                    }
                    catch (Exception finishException)
                    {
                        _logger.LogError("Failed to finish migration. Swallowing exception.", finishException);
                    }
                }
            }
            catch (DbMigrationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to upgrade database {db}", ex);
                throw new DbMigrationException($"Failed to upgrade database {db}", ex);
            }
        }

        private async Task<bool> LockMigration(
            Database db,
            DbMigrationsRepository dbMigrationsRepository,
            Guid migrationId)
        {
            try
            {
                await dbMigrationsRepository.SaveMigration(db, migrationId);

                var lockMigration = await dbMigrationsRepository.FindActiveMigration(db);

                // if the inserted migration doesn't match this migrationId, then another migration was already running 
                // and we failed to acquire the lock
                if (lockMigration != null && lockMigration.MigrationId == migrationId) return true;
                await dbMigrationsRepository.DeleteMigration(db, migrationId);
                return false;
            }
            catch (MySqlException mySqlException)
            {
                // ignore failures when table doesn't exist
                if (mySqlException.ErrorCode != MySqlErrorCode.NoSuchTable
                    && mySqlException.ErrorCode != MySqlErrorCode.UnknownDatabase)
                    throw;

                _logger.LogWarning(
                    $"{mySqlException.Message}. DANGEROUSLY allowing migration to run for the 1st time.");
                return true;
            }
            catch (SqlException sqlException)
            {
                _logger.LogWarning($"SQL Errors: {sqlException.Number}");
                // ignore failures when table doesn't exist
                if (sqlException.Number != 208 /* Invalid object name '%.*ls'. */)
                    throw;

                _logger.LogWarning(
                    $"{sqlException.Message}. DANGEROUSLY allowing migration to run for the 1st time.");
                return true;
            }
        }
    }
}