using System.ComponentModel;

namespace TripYari.Core.Extensions
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return default(T);
            }
        }

        public static string GetDescription(this Enum value)
        {
            if (value is null) return default;

            var attribute = value.GetAttribute<DescriptionAttribute>();

            return attribute is null ? value.ToString() : attribute.Description;
        }

        public static string[] ToArray(this Enum value)
        {
            return value?.ToString().Split(", ");
        }

        private static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            if (value is null) return default;

            var member = value.GetType().GetMember(value.ToString());

            var attributes = member[0].GetCustomAttributes(typeof(T), false);

            return (T)attributes[0];
        }
    }
}
