using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using TripYari.Core.Data.Abstraction.DbContexts;
using TripYari.Core.Data.Abstraction.Domain;
using TripYari.Core.Data.Abstraction.Repository;

namespace TripYari.Core.Data.Repository;

public abstract class CrudRepository<T, Tkey> : IRepository<T> where T : EntityBase<Tkey> where Tkey: struct
{
    private readonly ICommandRepository<T> _commandRepository;
    private readonly IQueryRepository<T> _queryRepository;

    protected CrudRepository
    (
        ICommandRepository<T> commandRepository,
        IQueryRepository<T> queryRepository
    )
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
    }

    protected CrudRepository(IUnitOfDbContext context)
    {
        this._commandRepository = new CommandRepository<T, Tkey>(context);
        this._queryRepository = new QueryRepository<T, Tkey>(context);
    }

    public IQueryable<T> Queryable => _queryRepository.Queryable;

    public DbContext RepoDbContext => throw new NotImplementedException();

    public void Add(T item) => _commandRepository.Add(item);

    public Task AddAsync(T item) => _commandRepository.AddAsync(item);

    public void AddRange(IEnumerable<T> items) => _commandRepository.AddRange(items);

    public Task AddRangeAsync(IEnumerable<T> items) => _commandRepository.AddRangeAsync(items);

    public Task<EntityEntry<T>> AddWithReturnAsync(T item) =>  _commandRepository.AddWithReturnAsync(item);

    public bool Any() => _queryRepository.Any();

    public bool Any(Expression<Func<T, bool>> where) => _queryRepository.Any(where);

    public Task<bool> AnyAsync() => _queryRepository.AnyAsync();

    public Task<bool> AnyAsync(Expression<Func<T, bool>> where) => _queryRepository.AnyAsync(where);

    public long Count() => _queryRepository.Count();

    public long Count(Expression<Func<T, bool>> where) => _queryRepository.Count(where);

    public Task<long> CountAsync() => _queryRepository.CountAsync();

    public Task<long> CountAsync(Expression<Func<T, bool>> where) => _queryRepository.CountAsync(where);

    public Task<List<T>> FromSqlRaw(string sql, params object[] parameters) => _queryRepository.FromSqlRaw(sql, parameters);

    public T? Get<Tkey>(Tkey key) where Tkey : struct => _queryRepository.Get(key);

    public Task<T?> GetAsync<Tkey>(Tkey key) where Tkey : struct => _queryRepository.GetAsync(key);

    public Task<T?> GetByIdAsync<Tkey>(Tkey id, params Expression<Func<T, object>>[] includeProperties) where Tkey : struct
    {
        return _queryRepository.GetByIdAsync(id, includeProperties);
    }

    public bool HardDelete(Expression<Func<T, bool>> where) => _commandRepository.HardDelete(where);

    public bool HardDelete(object key) => _commandRepository.HardDelete(key);

    public Task<bool> HardDeleteAsync(Expression<Func<T, bool>> where) => _commandRepository.HardDeleteAsync(where);

    public Task<bool> HardDeleteAsync(object key) => _commandRepository.HardDeleteAsync(key);

    public IEnumerable<T> List() => _queryRepository.List();

    public Task<IEnumerable<T>> ListAsync() => _queryRepository.ListAsync();

    public void Save() => _commandRepository.Save();

    public Task SaveChangesAsync() => _commandRepository.SaveChangesAsync();

    public void SoftDelete(Expression<Func<T, bool>> where) => _commandRepository.SoftDelete(where);

    public void SoftDelete(object key) => _commandRepository.SoftDelete(key);

    public Task SoftDeleteAsync(Expression<Func<T, bool>> where) => _commandRepository.SoftDeleteAsync(where);

    public Task SoftDeleteAsync(object key) => _commandRepository.SoftDeleteAsync(key);

    public void Update(T item) => _commandRepository.Update(item);

    public Task UpdateAsync(T item) => _commandRepository.UpdateAsync(item);

    public Task<T> UpdateFromResultAsync(T entity) => _commandRepository.UpdateFromResultAsync(entity);

    public void UpdatePartial(object item) => _commandRepository.UpdatePartial(item);

    public Task UpdatePartialAsync(object item) => _commandRepository.UpdatePartialAsync(item);

    public void UpdateRange(IEnumerable<T> items) => _commandRepository.UpdateRange(items);

    public Task UpdateRangeAsync(IEnumerable<T> items) => _commandRepository.UpdateRangeAsync(items);
}
