using TripYatri.Core.Data.DbMigrations.Services;
using TripYatri.Core.Data.DbMigrations.Services.DbMigrator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TripYatri.Core.Data.DbMigrations
{
    public static class ComponentProviderCollectionExtensions
    {
        public static IServiceCollection AddCdsDataDbMigrationsProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCdsDataProviders(configuration);

            services.AddTransient<IDbMigratorProvider, DbMigratorProvider>();

            services.AddHostedService<DbMigratorBackgroundProvider>();

            return services;
        }
    }
}
