using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;
using TripYatri.Core.Base.Providers.Metrics;
using TripYatri.Core.Data.Sevices;
using TripYatri.Core.Providers.MySql;

namespace TripYatri.Core.Data.Services.MySql
{
    public class MySqlRepository : IMySqlRepository
    {
        private readonly IDapperService _dapperProvider;
        private readonly ILogger _logger;
        private readonly IMetricsProvider _metricsProvider;

        public MySqlRepository(
            IServiceProvider serviceProvider,
            IDapperService dapperProvider)
        {
            _dapperProvider = dapperProvider;
            _logger = serviceProvider.GetRequiredService<ILogger>();
            _metricsProvider = serviceProvider.GetRequiredService<IMetricsProvider>();
        }

        public async Task<IEnumerable<PartitionName>> FindPartitionsOlderThan(
            Database database,
            TableName tableName,
            DateTimeOffset olderThan,
            int limit = 100)
        {
            var _ = _metricsProvider.BeginTiming(this);
            var partitionDescriptor = $"p{olderThan.Year * 100 + olderThan.Month}";

            _logger.LogInfo($"Finding partitions older than {partitionDescriptor} in table {tableName}");
            var partitions = await _dapperProvider.QueryAsync<string>(
                database,
                "SELECT PARTITION_NAME " +
                "FROM information_schema.partitions " +
                "WHERE TABLE_SCHEMA = @TableName " +
                "  AND TABLE_NAME = @TableName " +
                "  AND PARTITION_NAME < @PartitionDescriptor " +
                "LIMIT @Limit;",
                new
                {
                    tableName.Schema,
                    TableName = tableName.Name,
                    PartitionDescriptor = partitionDescriptor,
                    Limit = limit
                }
            );

            return partitions
                .Select(p => new PartitionName(tableName, p))
                .ToList();
        }

        public async Task DropPartition(Database database, PartitionName partitionName)
        {
            var _ = _metricsProvider.BeginTiming(this);
            _logger.LogWarning($"Dropping Partition ${partitionName}...");
            await _dapperProvider.ExecuteAsync(
                database,
                $"ALTER TABLE `{partitionName.Schema}`.`{partitionName.TableName}` TRUNCATE PARTITION `{partitionName.Name}`");
            await _dapperProvider.ExecuteAsync(
                database,
                $"ALTER TABLE `{partitionName.Schema}`.`{partitionName.TableName}` DROP PARTITION `{partitionName.Name}`");
        }
    }
}