using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace TripYari.Core.Data.Repository;

public interface ICommandRepository<T> where T : class
{
    void Add(T item);

    Task AddAsync(T item);

    void AddRange(IEnumerable<T> items);

    Task AddRangeAsync(IEnumerable<T> items);

    void Update(T item);

    Task UpdateAsync(T item);

    void UpdatePartial(object item);

    Task UpdatePartialAsync(object item);

    void UpdateRange(IEnumerable<T> items);

    Task UpdateRangeAsync(IEnumerable<T> items);
    bool HardDelete(Expression<Func<T, bool>> where);
    bool HardDelete(object key);
    Task<bool> HardDeleteAsync(Expression<Func<T, bool>> where);
    Task<bool> HardDeleteAsync(object key);
    void Save();
    Task SaveChangesAsync();
    void SoftDelete(Expression<Func<T, bool>> where);
    void SoftDelete(object key);
    Task SoftDeleteAsync(Expression<Func<T  , bool>> where);
    Task SoftDeleteAsync(object key);
    Task<EntityEntry<T>> AddWithReturnAsync(T item);
    Task<T> UpdateFromResultAsync(T entity);
}
