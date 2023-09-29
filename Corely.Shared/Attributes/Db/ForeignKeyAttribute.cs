namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        public string? Schema { get; }

        public string? Table { get; }

        public string[]? Columns { get; }

        public string? CustomSql { get; }

        public bool? Validate { get; init; }

        public ForeignKeyAttribute(string table, params string[] columns)
        {
            Table = table;
            Columns = columns;
        }

        public ForeignKeyAttribute(string schema, string table, params string[] columns)
            : this(table, columns)
        {
            Schema = schema;
        }

        public ForeignKeyAttribute(string customSql)
        {
            CustomSql = customSql;
        }
    }
}
