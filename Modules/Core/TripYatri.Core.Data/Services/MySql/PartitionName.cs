namespace TripYatri.Core.Providers.MySql
{
    public class PartitionName
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public string Name { get; set; }

        public PartitionName(string schema, string tableName, string name)
        {
            Schema = schema;
            TableName = tableName;
            Name = name;
        }

        public PartitionName(TableName tableName, string name)
        {
            Schema = tableName.Schema;
            TableName = tableName.Name;
            Name = name;
        }

        public override string ToString()
        {
            return $"`{Schema}`.`{TableName}`.`{Name}`";
        }
    }
}