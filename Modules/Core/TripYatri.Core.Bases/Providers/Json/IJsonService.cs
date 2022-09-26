using System.IO;
using Newtonsoft.Json;

namespace TripYatri.Core.Base.Providers.Json
{
    public interface IJsonProvider
    {
        string Serialize(object data);
        void Serialize(object data, Stream stream);
        T Deserialize<T>(string data);
        T Deserialize<T>(Stream stream);

        #region Configure methods

        IJsonProvider Indent();
        IJsonProvider WithSimpleIsoDateFormat();
        IJsonProvider WithCamelCase();
        IJsonProvider WithSnakeCase();
        IJsonProvider WithConverter(JsonConverter oConverter);

        #endregion
    }
}