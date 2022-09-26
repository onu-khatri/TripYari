using System.Globalization;
using Newtonsoft.Json.Converters;

namespace TripYatri.Core.Base.Providers.Json
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