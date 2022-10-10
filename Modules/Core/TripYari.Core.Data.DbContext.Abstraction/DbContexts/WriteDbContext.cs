using Microsoft.EntityFrameworkCore;

namespace TripYari.Core.Data.DbContexts.Abstraction
{
    public interface IReadWriteDbContext
    {
        DbContext context{ get; init; }
    }
}
