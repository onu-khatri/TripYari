using System;
using System.Data;
using Dapper;

namespace TripYatri.Core.MySql.TypeHandlers
{
    public class MySqlDateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = value;
        }

        public override DateTime Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime) value, DateTimeKind.Utc);
        }
    }
}