using Microsoft.EntityFrameworkCore;

namespace TripYari.Core.Data.DbContexts
{
    public interface IReadWriteDbContext
    {
        DbContext context{ get; init; }
    }
}
