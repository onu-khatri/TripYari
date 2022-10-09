﻿using TripYari.Core.Data.Abstraction.DataContextScope;
using TripYari.Core.Data.Abstraction.DbContexts;

namespace TripYari.Core.Data.DbContextScope
{
    public class AmbientDbContextLocator : IAmbientDbContextLocator
    {
        public TDbContext? Get<TDbContext>() where TDbContext : IUnitOfDbContext
        {
            var ambientDbContextScope = DbContextScope.GetAmbientScope();
            if (ambientDbContextScope == null)
                return default;

            IDbContextCollection dbContexts = ambientDbContextScope.DbContexts;
            return dbContexts.Get<TDbContext>();
        }
    }
}