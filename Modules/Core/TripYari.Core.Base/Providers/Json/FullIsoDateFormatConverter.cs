using System.Globalization;
using Newtonsoft.Json.Converters;

namespace TripYari.Core.Base.Providers.Json
{
    public class FullIsoDateFormatConverter : IsoDateTimeConverter
    {
        public FullIsoDateFormatConverter()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
            DateTimeStyles = DateTimeStyles.AdjustToUniversal;
        }
    }
}