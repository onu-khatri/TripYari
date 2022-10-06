using Microsoft.EntityFrameworkCore;

namespace TripYari.Core.Data.DbContexts
{
    public class UnitOfDbContext : IUnitOfDbContext
    {
        public IReadWriteDbContext ReadWriteContext { get; init; }
        public IReadDbContext? ReadContext { get; init; }

        public DbContext ReadWriteDbContext => ReadWriteContext.context;

        public UnitOfDbContext(IReadWriteDbContext writeContext, IReadDbContext readContext)
        {
            ReadWriteContext = writeContext;
            ReadContext = readContext;
        }

        public UnitOfDbContext(IReadWriteDbContext writeContext)
        {
            ReadWriteContext = writeContext;
        }

        public int SaveChanges()
        {
            return ReadWriteContext.context.SaveChanges();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return ReadWriteContext.context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            ReadWriteContext.context.Dispose();
            if (ReadContext == null)
            {
                ReadContext.context.Dispose();
            }
        }
    }
}
