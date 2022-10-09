using Microsoft.EntityFrameworkCore;

namespace TripYari.Core.Data.Abstraction.DbContexts
{
   // https://www.koskila.net/how-to-implement-multiple-dbcontexts-in-ef-core/
    public interface IReadDbContext
    {
        DbContext context { get; init; }
    }
}
