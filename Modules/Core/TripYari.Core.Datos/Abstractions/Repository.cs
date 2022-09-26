using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Travel.Core.Data.Extenstion;
using Travel.Core.Data.Interfaces;

namespace Travel.Core.Data.Abstractions
{
    public abstract class BaseRepository<TEntity, Key> : IRepository<TEntity, Key> where TEntity : EntityBase<Key> where Key : struct
    {
        private readonly DbContext RepoDbContext;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IQueryable<TEntity?> _queryable;

        public BaseRepository(DbContext context)
        {
            RepoDbContext = context;
            _dbSet = RepoDbContext.Set<TEntity>();
            _queryable = RepoDbContext.QuerySet<TEntity?>().Where(x => !x!.IsDeleted);
        }


        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            RepoDbContext.Entry(entity).State = EntityState.Deleted;
            return await Task.FromResult(entity);

        }

        public async Task<TEntity> GetByIdAsync(Key id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = RepoDbContext.Set<TEntity>().AsNoTracking() as IQueryable<TEntity>;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            return await queryable.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public async Task<List<TEntity>> FromSqlRaw(string sql, params object[] parameters)
        {
            return await RepoDbContext.Set<TEntity>().FromSqlRaw(sql, parameters).ToListAsync().ConfigureAwait(false);
        }


        public void Add(TEntity item) => _dbSet.Add(item);

        public async Task AddAsync(TEntity item) => await _dbSet.AddAsync(item);

        public async Task<EntityEntry<TEntity>> AddWithReturnAsync(TEntity item) => await _dbSet.AddAsync(item);

        public void AddRange(IEnumerable<TEntity> items) => _dbSet.AddRange(items);

        public async Task AddRangeAsync(IEnumerable<TEntity> items) => await _dbSet.AddRangeAsync(items);

        public void HardDelete(object key)
        {
            var entity = _dbSet.Find(key);
            if (entity == null)
                return;
            _dbSet.Remove(entity);
        }
        public void HardDelete(Expression<Func<TEntity, bool>> where)
        {
            var queryable = _dbSet.Where<TEntity>(where);
            if (!queryable.Any<TEntity>())
                return;
            _dbSet.RemoveRange(queryable);
        }
        public Task HardDeleteAsync(object key) => Task.Run((Action)(() => HardDelete(key)));
        public Task HardDeleteAsync(Expression<Func<TEntity, bool>> where) => Task.Run((Action)(() => HardDelete(where)));


        public void SoftDelete(object key)
        {
            var entity = _dbSet.Find(key);
            if (entity == null)
                return;
            entity.IsDeleted = true;
            RepoDbContext.Entry<TEntity>(entity).State = EntityState.Modified;
        }
        public void SoftDelete(Expression<Func<TEntity, bool>> where)
        {
            var queryable = _dbSet.Where<TEntity>(where);
            if (!queryable.Any<TEntity>())
                return;
            foreach (var entity in queryable)
            {
                entity.IsDeleted = true;
                RepoDbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            }
        }
        public Task SoftDeleteAsync(object key) => Task.Run((Action)(() => SoftDelete(key)));
        public Task SoftDeleteAsync(Expression<Func<TEntity, bool>> where) => Task.Run((Action)(() => SoftDelete(where)));

        public void Update(TEntity item)
        {
            var entity = _dbSet.Find(RepoDbContext.PrimaryKeyValues<TEntity>(item));
            if (entity == null)
                return;
            RepoDbContext.Entry<TEntity>(entity).State = EntityState.Detached;
            RepoDbContext.Update<TEntity>(item);
        }

        public Task UpdateAsync(TEntity item) => Task.Run((Action)(() => Update(item)));

        public async Task<TEntity> UpdateFromResultAsync(TEntity entity)
        {
            RepoDbContext.Entry(entity).State = EntityState.Modified;
            // await RepoDbContext.SaveChangesAsync();
            return await Task.FromResult(entity);
        }

        public void UpdatePartial(object item)
        {
            var entity = _dbSet.Find(RepoDbContext.PrimaryKeyValues<TEntity>(item));
            if (entity == null)
                return;
            var entityEntry = RepoDbContext.Entry<TEntity>(entity);
            entityEntry.CurrentValues.SetValues(item);
            foreach (var navigation in entityEntry.Metadata.GetNavigations())
            {
                if (!navigation.IsOnDependent && !((IReadOnlyNavigation)navigation).IsCollection &&
                    navigation.ForeignKey.IsOwnership)
                {
                    var property = item.GetType().GetProperty(navigation.Name);
                    if (property != null)
                    {
                        var obj = property.GetValue(item, null);
                        if (obj != null)
                            entityEntry.Reference(navigation.Name).TargetEntry?.CurrentValues.SetValues(obj);
                    }
                }
            }
        }

        public Task UpdatePartialAsync(object item) => Task.Run((Action)(() => UpdatePartial(item)));

        public void UpdateRange(IEnumerable<TEntity> items) => _dbSet.UpdateRange(items);

        public Task UpdateRangeAsync(IEnumerable<TEntity> items) => Task.Run((Action)(() => UpdateRange(items)));


        // Queries
        public bool Any() => _queryable!.Any<TEntity>();

        public bool Any(Expression<Func<TEntity, bool>> where) => _queryable!.Any<TEntity>(@where);

        public async Task<bool> AnyAsync() => await _queryable!.AnyAsync<TEntity>();

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where) => await _queryable!.AnyAsync<TEntity>(@where);

        public long Count() => _queryable!.LongCount<TEntity>();

        public long Count(Expression<Func<TEntity, bool>> where) => _queryable!.LongCount<TEntity>(@where);

        public async Task<long> CountAsync() => await _queryable!.LongCountAsync<TEntity>();

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> where) => await _queryable!.LongCountAsync<TEntity>(@where);

        public TEntity? Get(Guid key) => RepoDbContext.DetectChangesLazyLoading(false).Set<TEntity>().Find(key);

        public async Task<TEntity?> GetAsync(Key key) => await _queryable.Where(x => x!.Id.Equals(key)).SingleOrDefaultAsync();

        public IEnumerable<TEntity> List() => _queryable!.ToList<TEntity>();

        public async Task<IEnumerable<TEntity>> ListAsync() => await _queryable!.ToListAsync<TEntity>().ConfigureAwait(false);

        public async Task SaveChangesAsync() => await RepoDbContext.SaveChangesAsync();
        public void Save() => RepoDbContext.SaveChanges();
    }
}
