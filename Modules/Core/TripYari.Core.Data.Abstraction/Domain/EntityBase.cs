using System.ComponentModel.DataAnnotations;

namespace TripYari.Core.Data.Abstraction.Domain
{
    public abstract class EntityBase<T>:Base<EntityBase<T>>  where T : struct       
    {
        protected EntityBase() : this(IdProvider.GenerateId<T>())
        {
        }

        protected EntityBase(T id)
        {
            Id = id;
        }


        [Key]
        public T Id { get; protected set; }

        public bool IsDeleted { get; set; }

    }
}
