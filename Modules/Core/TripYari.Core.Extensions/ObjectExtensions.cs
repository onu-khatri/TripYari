using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace TripYari.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static byte[] Bytes(this object obj)
        {
            return Encoding.Default.GetBytes(JsonSerializer.Serialize(obj));
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<T>(this object obj) where T : Attribute
        {
            return obj.GetType().GetProperties().Where(property => Attribute.IsDefined(property, typeof(T)));
        }

        public static string Serialize(this object obj)
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static void SetProperty(this object obj, string name, object value)
        {
            obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)?.SetValue(obj, value);
        }
    }

}
