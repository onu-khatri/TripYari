using System.ComponentModel.DataAnnotations;

namespace Travel.Core.Data.Abstractions
{
    public abstract class EntityBase<T> where T : struct       
    {
        protected EntityBase() : this(IdProvider.GenerateId<T>())
        {
        }

        protected EntityBase(string Id) : this(IdProvider.GenerateId<T>())
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
