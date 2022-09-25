namespace TripYatri.Core.Providers.MySql
{
    public class AuroraTableName
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public AuroraTableName(string schema, string tableName)
        {
            Schema = schema;
            TableName = tableName;

        }
    }
}
