using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using TripYari.Core.Data.Abstraction.DbContexts;
using TripYari.Core.Data.Abstraction.Domain;
using TripYari.Core.Data.Abstraction.Repository;
using TripYari.Core.Data.Extenstion;

namespace TripYari.Core.Data.Repository
{
    public class CommandRepository<T, Tkey> : ICommandRepository<T> where T : EntityBase<Tkey> where Tkey: struct
    {
        private readonly DbContext _context;

        public CommandRepository(IUnitOfDbContext context)
        {
            _context = context.ReadWriteContext.context;
        }        

        private DbSet<T> Set => _context.CommandSet<T>();

        public void Add(T item) => Set.Add(item);

        public Task AddAsync(T item) => Set.AddAsync(item).AsTask();

        public void AddRange(IEnumerable<T> items) => Set.AddRange(items);

        public Task AddRangeAsync(IEnumerable<T> items) => Set.AddRangeAsync(items);

        public async Task<EntityEntry<T>> AddWithReturnAsync(T item) => await Set.AddAsync(item);

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }

        public bool HardDelete(object key)
        {
            var entity = Set.Find(key);
            if (entity == null)
                return false;
            Set.Remove(entity);
            return true;
        }

        public bool HardDelete(Expression<Func<T, bool>> where)
        {
            var queryable = Set.Where(where);
            if (!queryable.Any())
                return false;
            Set.RemoveRange(queryable);
            return true;
        }

        public Task<bool> HardDeleteAsync(object key) => Task.Run(() => HardDelete(key));
        public Task<bool> HardDeleteAsync(Expression<Func<T, bool>> where) => Task.Run(() => HardDelete(where));


        public void SoftDelete(object key)
        {
            var entity = Set.Find(key);
            if (entity == null)
                return;
            entity.IsDeleted = true;
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void SoftDelete(Expression<Func<T, bool>> where)
        {
            var queryable = Set.Where(where);
            if (!queryable.Any())
                return;
            foreach (var entity in queryable)
            {
                entity.IsDeleted = true;
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
        public Task SoftDeleteAsync(object key) => Task.Run(() => SoftDelete(key));
        public Task SoftDeleteAsync(Expression<Func<T, bool>> where) => Task.Run(() => SoftDelete(where));


        public void Update(T item)
        {
            var primaryKeyValues = _context.PrimaryKeyValues<T>(item);

            var entity = Set.Find(primaryKeyValues);

            if (entity is null) return;

            _context.Entry(entity).State = EntityState.Detached;

            _context.Update(item);
        }

        public Task UpdateAsync(T item)
        {
            return Task.Run(() => Update(item));
        }

        public async Task<T> UpdateFromResultAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await Task.FromResult(entity);
        }

        public void UpdatePartial(object item)
        {
            var primaryKeyValues = _context.PrimaryKeyValues<T>(item);

            var entity = Set.Find(primaryKeyValues);

            if (entity is null) return;

            var entry = _context.Entry(entity);

            entry.CurrentValues.SetValues(item);

            foreach (var navigation in entry.Metadata.GetNavigations())
            {
                if (navigation.IsOnDependent || navigation.IsCollection || !navigation.ForeignKey.IsOwnership) continue;

                var property = item.GetType().GetProperty(navigation.Name);

                if (property is null) continue;

                var value = property.GetValue(item, default);

                entry.Reference(navigation.Name).TargetEntry?.CurrentValues.SetValues(value!);
            }
        }

        public Task UpdatePartialAsync(object item) => Task.Run(() => UpdatePartial(item));

        public void UpdateRange(IEnumerable<T> items) => Set.UpdateRange(items);

        public Task UpdateRangeAsync(IEnumerable<T> items) => Task.Run(() => UpdateRange(items));
    }
}
