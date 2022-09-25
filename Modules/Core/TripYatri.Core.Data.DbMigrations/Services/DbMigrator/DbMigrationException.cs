using System;

namespace TripYatri.Core.Data.DbMigrations.Services.DbMigrator
{
    public class DbMigrationException : Exception
    {
        public DbMigrationException(string message) : base(message)
        {
        }

        public DbMigrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}