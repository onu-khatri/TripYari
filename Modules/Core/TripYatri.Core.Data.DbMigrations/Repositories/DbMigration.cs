using System;
using Dapper;

namespace TripYatri.Core.Data.DbMigrations.Repositories
{
    public class DbMigration
    {
        public long Id { get; set; }
        public Guid MigrationId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset CheckInTime { get; set; }
        public DateTimeOffset FinishTime { get; set; }

        public class Query
        {
            public long? Id { get; set; }
            public DbString MigrationId { get; set; }
            public DateTimeOffset? StartTime { get; set; }
            public DateTimeOffset? CheckInTime { get; set; }
            public DateTimeOffset? FinishTime { get; set; }

            public Query(
                long? id = null,
                Guid? migrationId = null, 
                DateTimeOffset? startTime = null, 
                DateTimeOffset? checkInTime = null,
                DateTimeOffset? finishTime = null)
            {
                Id = id;

                if (migrationId.HasValue)
                    MigrationId = new DbString
                    {
                        IsAnsi = true,
                        IsFixedLength = false,
                        Length = 50,
                        Value = migrationId.ToString()
                    };

                StartTime = startTime;
                CheckInTime = checkInTime;
                FinishTime = finishTime;
            }
        }
    }
}