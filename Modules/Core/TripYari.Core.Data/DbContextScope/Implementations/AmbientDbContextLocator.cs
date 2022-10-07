using TripYari.Core.Data.DbContexts;

namespace TripYari.Core.Data.DataContextScope.Entity
{
    public class AmbientDbContextLocator : IAmbientDbContextLocator
    {
        public TDbContext? Get<TDbContext>() where TDbContext : IUnitOfDbContext
        {
            var ambientDbContextScope = DbContextScope.GetAmbientScope();
            if (ambientDbContextScope == null)
                return default;

            IDbContextCollection dbContexts = ambientDbContextScope.DbContexts;
            return dbContexts.Get<TDbContext>();
        }
    }
}