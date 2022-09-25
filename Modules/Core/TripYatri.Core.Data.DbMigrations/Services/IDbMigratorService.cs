using System.Collections.Generic;
using System.Threading.Tasks;

namespace TripYatri.Core.Data.DbMigrations.Services
{
    public interface IDbMigratorProvider
    {
        Task Migrate(Database db, IDictionary<string, string> migrationVariables = null);
    }
}