using Microsoft.EntityFrameworkCore;

namespace TripYari.Core.Data.Abstraction.DbContexts
{
    public interface IReadWriteDbContext
    {
        DbContext context{ get; init; }
    }
}
