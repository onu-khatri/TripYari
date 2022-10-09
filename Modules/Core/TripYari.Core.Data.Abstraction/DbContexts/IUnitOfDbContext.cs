using Microsoft.EntityFrameworkCore;

namespace TripYari.Core.Data.Abstraction.DbContexts
{
    public interface IUnitOfDbContext: IDisposable
    {
        IReadDbContext? ReadContext { get; init; }
        IReadWriteDbContext ReadWriteContext { get; init; }
        DbContext ReadWriteDbContext { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}