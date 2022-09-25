using System;
using TripYatri.Core.MySql.TypeHandlers;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TripYatri.Core.Data
{
    public static class ComponentProviderCollectionExtensions
    {
        public static IServiceCollection AddCdsDataProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DataOptions>(configuration.GetSection("Data"));

            services.AddScoped<IDataContextFactory, DataContextFactory>();

            // Support mapping of mysql snake-cased columns
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            
            // Add support for Dapper VARCHAR-to-Guid deserialization
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            // MySql doesn't support date offsets, so all dates are stored as UTC but read as local by default. 
            // These handlers correct that behavior
            SqlMapper.AddTypeHandler(new MySqlDateTimeHandler());
            SqlMapper.AddTypeHandler(new MySqlDateTimeOffsetHandler());

            return services;
        }
    }
}
