using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Travel.Core.Data.Abstractions;
using Travel.Core.Data.Interfaces;
using TripYari.Core.Datos.UnitOfWork;

namespace Travel.Core.Data.Extenstion
{
    public static class RepositoryExtensions
    {
        public static void AddContext<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where T : DbContext
        {
            services.AddDbContextPool<T>(options);
            services.BuildServiceProvider().GetRequiredService<T>().Database.Migrate();
            services.AddScoped<IUnitOfWork, UnitOfWork<T>>();
        }

        public static void AddContextMemory<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddDbContextPool<T>(delegate (DbContextOptionsBuilder options)
            {
                options.UseInMemoryDatabase(typeof(T).Name);
            });
            services.BuildServiceProvider().GetRequiredService<T>().Database.EnsureCreated();
            services.AddScoped<IUnitOfWork, UnitOfWork<T>>();
        }

        public static DbSet<T> CommandSet<T>(this DbContext context) where T : class
        {
            return context.DetectChangesLazyLoading(enabled: true).Set<T>();
        }

        public static IQueryable<T> QuerySet<T>(this DbContext context) where T : class
        {
            return context.DetectChangesLazyLoading(enabled: false).Set<T>().AsNoTracking();
        }

        public static DbContext DetectChangesLazyLoading(this DbContext context, bool enabled)
        {
            context.ChangeTracker.AutoDetectChangesEnabled = enabled;
            context.ChangeTracker.LazyLoadingEnabled = enabled;
            context.ChangeTracker.QueryTrackingBehavior = ((!enabled) ? QueryTrackingBehavior.NoTracking : QueryTrackingBehavior.TrackAll);
            return context;
        }

        public static object[] PrimaryKeyValues<T>(this DbContext context, object entity)
        {
            return context.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties.Select((IProperty property) => entity.GetType().GetProperty(property.Name)?.GetValue(entity, null)).ToArray();
        }

        public static async Task<TEntity> FindOneAsync<TEntity, Key>(
            this IRepository<TEntity, Key> repo,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
            where TEntity : EntityBase<Key>
            where Key : struct
        {
            var dbSet = repo.RepoDbContext.Set<TEntity>() as IQueryable<TEntity>;
            foreach (var includeProperty in includeProperties)
            {
                dbSet = dbSet.Include(includeProperty);
            }

            return await dbSet.FirstOrDefaultAsync(filter);
        }

      
    }
}
