using System;

namespace TripYatri.Core.Base
{
    public class RuntimeContextSettings
    {
        public string Team { get; set; }
        public string Application { get; set; }
        public string BuildVersion { get; set; }
        public TimeSpan ContextLifeSpan { get; set; }
    }
}