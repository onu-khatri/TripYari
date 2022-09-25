using System;
using System.Collections.Generic;
using System.Text;

namespace TripYatri.Core.Data
{
    public class Database
    {
        public string Name { get; set; }
        public DatabaseProvider DatabaseProviderType { get; set; }

        public Database()
        {
            DatabaseProviderType = DatabaseProvider.MySql;
        }
    }
}
