using System;

namespace TripYatri.Core.Base
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


    }
}
