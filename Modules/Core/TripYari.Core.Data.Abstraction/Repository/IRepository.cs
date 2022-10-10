﻿namespace TripYari.Core.Data.Abstraction.Repository
{
    public interface IRepository<TEntity>: ICommandRepository<TEntity>, IQueryRepository<TEntity> where TEntity : class
    {      
        
    }
}
