using System;
using System.Linq;
using System.Threading.Tasks;
using TripYatri.Core.Data.Sevices;

namespace TripYatri.Core.Data.DbMigrations.Repositories
{
    public class DbMigrationsRepository : IDataRepository
    {
        private readonly IDapperService _dapperProvider;

        public DbMigrationsRepository(
            IServiceProvider serviceProvider,
            IDapperService dapperProvider)
        {
            _dapperProvider = dapperProvider;
        }

        public async Task SaveMigration(Database db, Guid migrationId)
        {
            await _dapperProvider.ExecuteAsync(
                db,
                $"INSERT INTO db_migrations (migration_id, start_time, checkin_time) " +
                $"VALUES (" +
                $"  @{nameof(DbMigration.Query.MigrationId)}, " +
                $"  @{nameof(DbMigration.Query.StartTime)}, " +
                $"  @{nameof(DbMigration.Query.CheckInTime)}" +
                $")",
                new DbMigration.Query(
                    migrationId: migrationId,
                    startTime: DateTimeOffset.Now,
                    checkInTime: DateTimeOffset.Now
                )
            );
            _dapperProvider.Commit();
        }

        public async Task<DbMigration> FindActiveMigration(Database db)
        {
            string sqlQuery;
            switch (db.DatabaseProviderType)
            {
                case DatabaseProvider.MySql:
                    sqlQuery = $"SELECT * FROM db_migrations " +
                               $"WHERE checkin_time > @{nameof(DbMigration.Query.CheckInTime)} " +
                               $"  AND finish_time IS NULL " +
                               $"ORDER BY start_time ASC " +
                               $"LIMIT 1;";
                    break;
                case DatabaseProvider.SqlServer:
                    sqlQuery = $"SELECT TOP 1 * FROM db_migrations " +
                               $"WHERE checkin_time > @{nameof(DbMigration.Query.CheckInTime)} " +
                               $"  AND finish_time IS NULL " +
                               $"ORDER BY start_time ASC";
                    break;
                default:
                    throw new NotImplementedException($"Unsupported provider for {db}: {db.DatabaseProviderType}");
            }

            return (
                await _dapperProvider.QueryAsync<DbMigration>(
                    db,
                    sqlQuery,
                    new DbMigration.Query(
                        checkInTime: DateTimeOffset.Now.AddMinutes(-2)
                    )
                )
            ).FirstOrDefault();
        }

        public async Task DeleteMigration(Database db, Guid migrationId)
        {
            await _dapperProvider.ExecuteAsync(
                db,
                $"DELETE FROM db_migrations " +
                $"WHERE migration_id = @{nameof(DbMigration.Query.MigrationId)}",
                new DbMigration.Query(
                    migrationId: migrationId
                )
            );
        }

        public async Task CheckIn(Database db, Guid migrationId)
        {
            await _dapperProvider.ExecuteAsync(
                db,
                $"UPDATE db_migrations " +
                $"SET checkin_time = @{nameof(DbMigration.Query.CheckInTime)} " +
                $"WHERE migration_id = @{nameof(DbMigration.Query.MigrationId)}",
                new DbMigration.Query(
                    migrationId: migrationId,
                    checkInTime: DateTimeOffset.Now
                )
            );
        }

        public async Task FinishMigration(Database db, Guid migrationId)
        {
            await _dapperProvider.ExecuteAsync(
                db,
                $"UPDATE db_migrations " +
                $"SET finish_time = @{nameof(DbMigration.Query.FinishTime)} " +
                $"WHERE migration_id = @{nameof(DbMigration.Query.MigrationId)}",
                new DbMigration.Query(
                    migrationId: migrationId,
                    finishTime: DateTimeOffset.Now
                )
            );
        }
    }
}