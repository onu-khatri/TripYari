using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using TripYari.Core.Data.Abstraction.Domain;
using TripYari.Core.Data.Abstraction.Repository;

namespace TripYari.Core.Data.Extenstion
{
    public static class DbContextExtensions
    {
        public static void AddContext<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where T : DbContext
        {
            services.AddDbContextPool<T>(options);
            services.BuildServiceProvider().GetRequiredService<T>().Database.Migrate();
        }

        public static void AddContextMemory<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddDbContextPool<T>(delegate (DbContextOptionsBuilder options)
            {
                options.UseInMemoryDatabase(typeof(T).Name);
            });
            services.BuildServiceProvider().GetRequiredService<T>().Database.EnsureCreated();
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

        

      
    }
}
