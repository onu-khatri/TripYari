using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripYatri.Core.Providers.MySql;

namespace TripYatri.Core.Data.Services
{
    public interface IMySqlRepository : IDataRepository
    {
        Task<IEnumerable<PartitionName>> FindPartitionsOlderThan(
            Database database,
            TableName tableName, 
            DateTimeOffset olderThan,
            int limit = 100);
    
        Task DropPartition(Database database, PartitionName partitionName);
    }
}