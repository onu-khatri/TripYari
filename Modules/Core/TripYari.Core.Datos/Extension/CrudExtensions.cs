using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Travel.Core.Data.Abstractions;
using Travel.Core.Data.Interfaces;

namespace Travel.Core.Data.Extenstion
{


    public static class CrudExtensions
    {
        public static async Task<TCreateItemResponse> CreateItemFlowAsync<TEntity, Key, TCreateItemRequest, TCreateItemResponse>(
            this IRepository<TEntity, Key> repo,
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

       

        public static async Task<TRetrieveItemResponse> RetrieveItemFlowAsync<TEntity, Key, TRetrieveItemResponse>(
            this IRepository<TEntity, Key> repo, Key id,
            Func<TEntity, TRetrieveItemResponse> mapDataFunc)
            where TEntity : EntityBase<Key>
            where Key : struct

        {
            var retrieved = await repo.GetByIdAsync(id);
            return mapDataFunc(retrieved);
        }

       

        public static async Task<TUpdateItemResponse> UpdateItemFlowAsync<TEntity, Key, TUpdateItemResponse>(
            this IRepository<TEntity, Key> repo,
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
            this IRepository<TEntity, Key> repo,
            Key id,
            Func<TEntity, TDeleteItemResponse> mapResponseFunc,
            Action<TEntity> raiseEventAction = null,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : EntityBase<Key>
            where Key : struct
        {
            var item = await repo.GetByIdAsync(id, includes);
            var itemDeleted = await repo.DeleteAsync(item);
            raiseEventAction?.Invoke(itemDeleted);
            return mapResponseFunc(itemDeleted);
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
