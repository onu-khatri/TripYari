namespace TripYatri.Core.Providers.MySql
{
    public class TableName
    {
        public string Schema { get; set; }
        public string Name { get; set; }

        public TableName(string schema, string name)
        {
            Schema = schema;
            Name = name;
        }

        public override string ToString()
        {
            return $"`{Schema}`.`{Name}`";
        }
    }
}