using System;

namespace Travel.Core.Data.Abstractions
{
    
    public static class IdProvider
    {

        public static T GenerateId<T>() where T : struct
        {
            return (T)(typeof(T) == typeof(Guid) ? (object)Guid.NewGuid() : default(T));
        }
        
    }
}
