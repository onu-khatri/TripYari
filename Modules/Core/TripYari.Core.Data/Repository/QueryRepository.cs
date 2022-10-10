using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TripYari.Core.Data.Abstraction.Domain;
using TripYari.Core.Data.Abstraction.Repository;
using TripYari.Core.Data.DbContexts.Abstraction;
using TripYari.Core.Data.Extenstion;

namespace TripYari.Core.Data.Repository
{
    public class QueryRepository<T, Tkey> : IQueryRepository<T> where T : EntityBase<Tkey> where Tkey : struct
    {
        private readonly DbContext _context;

        public QueryRepository(IUnitOfDbContext context)
        {
            _context = context.ReadContext == null ? context.ReadWriteContext.context : context.ReadContext.context;
        }
        public IQueryable<T> Queryable => _context.QuerySet<T>();

        public bool Any() => Queryable.Any();

        public bool Any(Expression<Func<T, bool>> where) => Queryable.Any(where);

        public Task<bool> AnyAsync() => Queryable.AnyAsync();

        public Task<bool> AnyAsync(Expression<Func<T, bool>> where) => Queryable.AnyAsync(where);

        public long Count() => Queryable.LongCount();

        public long Count(Expression<Func<T, bool>> where) => Queryable.LongCount(where);

        public Task<long> CountAsync() => Queryable.LongCountAsync();

        public Task<long> CountAsync(Expression<Func<T, bool>> where) => Queryable.LongCountAsync(where);

        public async Task<List<T>> FromSqlRaw(string sql, params object[] parameters)
        {
            return await _context.Set<T>().FromSqlRaw(sql, parameters).ToListAsync().ConfigureAwait(false);
        }

        public T? Get<Tkey>(Tkey key) where Tkey : struct => _context.DetectChangesLazyLoading(false).Set<T>().Find(key);

        public Task<T?> GetAsync<Tkey>(Tkey key) where Tkey : struct => _context.DetectChangesLazyLoading(false).Set<T>().FindAsync(key).AsTask();

        public async Task<T?> GetByIdAsync<key>(key id, params Expression<Func<T, object>>[] includeProperties) where key: struct
        {
            var queryable = _context.Set<T>().AsNoTracking() as IQueryable<T>;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            return await queryable.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public IEnumerable<T> List() => Queryable.ToList();

        public async Task<IEnumerable<T>> ListAsync() => await Queryable.ToListAsync().ConfigureAwait(false);
    }
}
