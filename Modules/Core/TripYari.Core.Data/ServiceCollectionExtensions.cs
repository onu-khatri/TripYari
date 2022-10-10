﻿using Microsoft.Extensions.DependencyInjection;
using TripYari.Core.Data.Repository;
using TripYari.Core.Data.DbContexts;
using TripYari.Core.Data.DbContexts.Abstraction;
using TripYari.Core.Data.DbContexts.Abstraction.DataContextScope;
using TripYari.Core.Data.Abstraction.Domain;
using TripYari.Core.Data.DbContextScope;

namespace TripYari.Core.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            var dbContextFactory = new RegisteredDbContextFactory();
            services.AddScoped<IUnitOfDbContext, UnitOfDbContext>();
            services.AddSingleton<IAmbientDbContextLocator, AmbientDbContextLocator>();
            services.AddSingleton<IDbContextFactory>(dbContextFactory);
            return services;
        }

        public static IServiceCollection AddAllBaseRepositories<TEntity, key>(this IServiceCollection services)
        where TEntity : EntityBase<key>
        where key : struct
        {
            services.AddCommandRepository<TEntity, key, CommandRepository<TEntity, key>>();
            services.AddQueryRepository<TEntity, key, QueryRepository<TEntity, key>>();
            services.AddCrudRepository<TEntity, key, CrudRepository<TEntity, key>>();
            return services;
        }

        private static IServiceCollection AddCrudRepository<TEntity, key, TRepository>(this IServiceCollection services)
        where TEntity : EntityBase<key>
        where key : struct
        where TRepository : CrudRepository<TEntity, key>
        {
            services.AddScoped<CrudRepository<TEntity, key>, TRepository>();
            return services;
        }

        public static IServiceCollection AddCommandRepository<TEntity, key, TRepository>(this IServiceCollection services)
       where TEntity : EntityBase<key>
       where key : struct
       where TRepository : CommandRepository<TEntity, key>
        {
            services.AddScoped<CommandRepository<TEntity, key>, TRepository>();
            return services;
        }

        public static IServiceCollection AddQueryRepository<TEntity, key, TRepository>(this IServiceCollection services)
       where TEntity : EntityBase<key>
       where key : struct
       where TRepository : QueryRepository<TEntity, key>
        {
            services.AddScoped<QueryRepository<TEntity, key>, TRepository>();
            return services;
        }
    }
}
