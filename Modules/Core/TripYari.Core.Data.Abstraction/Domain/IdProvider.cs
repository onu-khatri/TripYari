namespace TripYari.Core.Data.Abstraction.Domain
{

    public static class IdProvider
    {

        public static T GenerateId<T>() where T : struct
        {
            return (T)(typeof(T) == typeof(Guid) ? (object)Guid.NewGuid() : default(T));
        }
        
    }
}
