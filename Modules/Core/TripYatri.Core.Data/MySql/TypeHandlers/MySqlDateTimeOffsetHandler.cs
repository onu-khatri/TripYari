using System;
using System.Data;
using Dapper;

namespace TripYatri.Core.MySql.TypeHandlers
{
    public class MySqlDateTimeOffsetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            parameter.Value = value;
        }

        public override DateTimeOffset Parse(object value)
        {
            return new DateTimeOffset(DateTime.SpecifyKind((DateTime) value, DateTimeKind.Utc));
        }
    }
}