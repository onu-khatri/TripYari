using System;

namespace TripYatri.Core.Data
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DatabaseAttribute : Attribute
    {
        public DatabaseProvider Provider { get; }

        public DatabaseAttribute(DatabaseProvider provider)
        {
            Provider = provider;
        }
    }
}