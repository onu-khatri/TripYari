namespace TripYari.Core.Data.Domain
{
    public abstract class ValueObject<T>
    {
        protected ValueObject(T value) => Value = value;

        public T Value { get; }
    }
}
