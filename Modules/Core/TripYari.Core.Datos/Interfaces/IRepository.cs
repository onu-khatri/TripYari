using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Travel.Core.Data.Abstractions;
using Travel.Core.Data.Models.Results;

namespace Travel.Core.Data.Interfaces
{
    public interface IRepository<TEntity, Key> where TEntity : EntityBase<Key> where Key : struct
    {
        void Add(TEntity item);
        Task AddAsync(TEntity item);
        Task<EntityEntry<TEntity>> AddWithReturnAsync(TEntity item);
        Task<TEntity> UpdateFromResultAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> items);
        Task AddRangeAsync(IEnumerable<TEntity> items);
        bool Any();
        bool Any(Expression<Func<TEntity, bool>> where);
        Task<bool> AnyAsync();
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
        long Count();
        long Count(Expression<Func<TEntity, bool>> where);
        Task<long> CountAsync();
        Task<long> CountAsync(Expression<Func<TEntity, bool>> where);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<List<TEntity>> FromSqlRaw(string sql, params object[] parameters);
        TEntity? Get(Guid key);
        Task<TEntity?> GetAsync(Key key);
        Task<TEntity> GetByIdAsync(Key id, params Expression<Func<TEntity, object>>[] includeProperties);
        void HardDelete(Expression<Func<TEntity, bool>> where);
        void HardDelete(object key);
        Task HardDeleteAsync(Expression<Func<TEntity, bool>> where);
        Task HardDeleteAsync(object key);
        IEnumerable<TEntity> List();
        Task<IEnumerable<TEntity>> ListAsync();
        void Save();
        Task SaveChangesAsync();
        void SoftDelete(Expression<Func<TEntity, bool>> where);
        void SoftDelete(object key);
        Task SoftDeleteAsync(Expression<Func<TEntity, bool>> where);
        Task SoftDeleteAsync(object key);
        void Update(TEntity item);
        Task UpdateAsync(TEntity item);
        void UpdatePartial(object item);
        Task UpdatePartialAsync(object item);
        void UpdateRange(IEnumerable<TEntity> items);
        Task UpdateRangeAsync(IEnumerable<TEntity> items);
    }
}
