using System.Globalization;
using Newtonsoft.Json.Converters;

namespace TripYari.Core.Base.Providers.Json
{
    public class SimpleIsoDateFormatConverter : IsoDateTimeConverter
    {
        public SimpleIsoDateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";
            DateTimeStyles = DateTimeStyles.AdjustToUniversal;
        }
    }
}