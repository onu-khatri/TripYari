using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace TripYari.Core.Base.Providers.Json
{
    public class NewtonsoftJsonProvider : IJsonProvider
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public NewtonsoftJsonProvider()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                Formatting = Formatting.None,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
            };
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, _jsonSerializerSettings);
        }

        public void Serialize(object data, Stream stream)
        {
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);

            using var sw = new StreamWriter(stream);
            using var jsonTextWriter = new JsonTextWriter(sw);
            serializer.Serialize(jsonTextWriter, data);
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, _jsonSerializerSettings);
        }

        public T Deserialize<T>(Stream stream)
        {
            using var sr = new StreamReader(stream);
            using var reader = new JsonTextReader(sr);
            var serializer = JsonSerializer.Create(_jsonSerializerSettings);
            return serializer.Deserialize<T>(reader);
        }

        public IJsonProvider Indent()
        {
            _jsonSerializerSettings.Formatting = Formatting.Indented;
            return this;
        }

        public IJsonProvider WithSimpleIsoDateFormat()
        {
            _jsonSerializerSettings.Converters.Add(new SimpleIsoDateFormatConverter());
            return this;
        }

        public IJsonProvider WithCamelCase()
        {
            (_jsonSerializerSettings.ContractResolver as DefaultContractResolver)
                .NamingStrategy = new CamelCaseNamingStrategy();
            return this;
        }

        public IJsonProvider WithSnakeCase()
        {
            (_jsonSerializerSettings.ContractResolver as DefaultContractResolver)
                .NamingStrategy = new SnakeCaseNamingStrategy();
            return this;
        }

        public IJsonProvider WithConverter(JsonConverter oConverter)
        {
            _jsonSerializerSettings.Converters.Add(oConverter);
            return this;
        }
    }
}