using DbUp.Engine.Output;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Data.DbMigrations.Services.DbMigrator
{
    public class DbMigratorUpgradeLog : IUpgradeLog
    {
        private readonly ILogger _logger;

        public DbMigratorUpgradeLog(ILogger logger)
        {
            _logger = logger;
        }
        
        public void WriteInformation(string format, params object[] args)
        {
            _logger.LogInfo(string.Format(format, args));
        }

        public void WriteError(string format, params object[] args)
        {
            _logger.LogError(string.Format(format, args));
        }

        public void WriteWarning(string format, params object[] args)
        {
            _logger.LogWarning(string.Format(format, args));
        }
    }
}