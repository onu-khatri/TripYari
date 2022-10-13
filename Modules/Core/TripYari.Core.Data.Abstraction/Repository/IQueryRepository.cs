using System.Linq.Expressions;
using TripYari.Core.Data.Abstraction.Specifications;

namespace TripYari.Core.Data.Abstraction.Repository;

public interface IQueryRepository<T> where T : class
{
    IQueryable<T> Queryable { get; }

    Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec);

    Task<int> CountAsync(ISpecification<T> spec);

    bool Any();

    bool Any(Expression<Func<T, bool>> where);

    Task<bool> AnyAsync();

    Task<bool> AnyAsync(Expression<Func<T, bool>> where);

    long Count();

    long Count(Expression<Func<T, bool>> where);

    Task<long> CountAsync();

    Task<long> CountAsync(Expression<Func<T, bool>> where);

    IEnumerable<T> List();

    Task<IEnumerable<T>> ListAsync();
    Task<List<T>> FromSqlRaw(string sql, params object[] parameters);
    T? Get<Tkey>(Tkey key) where Tkey: struct ;
    Task<T?> GetAsync<Tkey>(Tkey key) where Tkey : struct;
    Task<T?> GetByIdAsync<Tkey>(Tkey id, params Expression<Func<T, object>>[] includeProperties) where Tkey : struct;
}
