using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TripYari.Core.Data.Abstraction.Domain;
using TripYari.Core.Data.Abstraction.Repository;

namespace TripYari.Core.Data.Abstraction.Extenstion
{


    public static class RepositoryExtensions
    {
        public static async Task<TCreateItemResponse> CreateItemFlowAsync<TEntity, Key, TCreateItemRequest, TCreateItemResponse>(
            this IRepository<TEntity> repo,
            TCreateItemRequest request,
            Func<TCreateItemRequest, TEntity> mapCreateItemFunc,
            Func<TEntity, Task<TCreateItemResponse>> mapResponseFunc,
            Action<TEntity> raiseEventAction = null)
            where TEntity : EntityBase<Key>
            where Key : struct
            where TCreateItemRequest : IRequest<TCreateItemResponse>
        {
            var createEntity = mapCreateItemFunc(request);
            var entityCreated = await repo.AddWithReturnAsync(createEntity);
            raiseEventAction?.Invoke(createEntity);
            return await mapResponseFunc(createEntity);
        }

        public static async Task<TEntity> FindOneAsync<TEntity, Key>(
             this IRepository<TEntity> repo,
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

        public static async Task<TRetrieveItemResponse> RetrieveItemFlowAsync<TEntity, Key, TRetrieveItemResponse>(
            this IRepository<TEntity> repo, Key id,
            Func<TEntity, TRetrieveItemResponse> mapDataFunc)
            where TEntity : EntityBase<Key>
            where Key : struct

        {
            var retrieved = await repo.GetByIdAsync(id);
            return mapDataFunc(retrieved);
        }

       

        public static async Task<TUpdateItemResponse> UpdateItemFlowAsync<TEntity, Key, TUpdateItemResponse>(
            this IRepository<TEntity> repo,
            Key id,
            Func<TEntity, TEntity> updateMappingFunc,
            Func<TEntity, TUpdateItemResponse> mapResponseFunc,
            Action<TEntity> raiseEventAction = null,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : EntityBase<Key>
            where Key : struct

        {
            var item = await repo.GetByIdAsync(id, includes);
            var itemMapped = updateMappingFunc(item);
            var itemUpdated = await repo.UpdateFromResultAsync(itemMapped);
            raiseEventAction?.Invoke(itemUpdated);
            return mapResponseFunc(itemUpdated);
        }

        public static async Task<TDeleteItemResponse> DeleteItemFlowAsync<TEntity, Key, TDeleteItemResponse>(
            this IRepository<TEntity> repo,
            Key id,
            Func<TEntity, TDeleteItemResponse> mapResponseFunc,
            Action<TEntity> raiseEventAction = null,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : EntityBase<Key>
            where Key : struct
        {
            var item = await repo.GetByIdAsync(id, includes);
            var itemDeleted = await repo.HardDeleteAsync(item);
            raiseEventAction?.Invoke(item);
            return mapResponseFunc(item);
        }


        public static async Task ValidateRequestAsync<TItemRequest>(this IValidator<TItemRequest> validator, TItemRequest request)
        {
            var failureTask = await validator.ValidateAsync(request);
            var failures = failureTask.Errors
                .Where(error => error != null)
                .ToList();

            if (failures.Count > 0)
            {
                throw new ValidationException("[CRUD] Validation Exception.", failures);
            }
        }
    }
}
