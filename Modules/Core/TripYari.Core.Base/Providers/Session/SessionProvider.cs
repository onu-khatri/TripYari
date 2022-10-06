using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;

namespace TripYari.Core.Base.Providers.Session
{
    public class SessionProvider: ISessionProvider
    {
        private static IHttpContextAccessor _httpContextAccessor;
        private static ISession _sessionContext => _httpContextAccessor?.HttpContext?.Session;

        public SessionProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Set(string key, object data)
        {
            _sessionContext.Set(key, ObjectToByteArray(data));
        }

        public T Get<T>(string key)
        {
            if( _sessionContext.TryGetValue(key, out byte[] sessionData))
            {
                try
                {
                    var data = ByteArrayToObject(sessionData);
                    return (data is T) ? (T)data : default(T);
                }
                catch (Exception ex)
                {

                }
            }

            return default(T);
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        // Convert a byte array to an Object
        private object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = binForm.Deserialize(memStream);

            return obj;
        }
    }
}
