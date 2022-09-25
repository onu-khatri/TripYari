using System.Collections.Generic;

namespace TripYatri.Core.Data
{
    public class DataOptions
    {
        public Dictionary<string, ConnectionStringOptions> Connections { get; set; } =
            new Dictionary<string, ConnectionStringOptions>();

        public class ConnectionStringOptions
        {
            public Database databaseType { get; set; }
            public string ConnectionString { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string Collation { get; set; }
            public bool EnableMigrations { get; set; } = true;
        }
    }
}