namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        public string Table { get; }

        public string[] Columns { get; }

        public string? Schema { get; }

        public string? CustomSql { get; }

        public bool? Validate { get; init; }

        public ForeignKeyAttribute(string table, params string[] columns)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(table, nameof(table));
            ArgumentNullException.ThrowIfNull(columns, nameof(columns));
            columns.ToList().ForEach(c => ArgumentException.ThrowIfNullOrWhiteSpace(c, nameof(columns)));

            Table = table;
            Columns = columns;
        }

        public ForeignKeyAttribute(string schema, string table, params string[] columns)
            : this(table, columns)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(schema, nameof(schema));
            Schema = schema;
        }

        public ForeignKeyAttribute(string customSql)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(customSql, nameof(customSql));
            CustomSql = customSql;
        }

        public bool IsCustom()
        {
            return !string.IsNullOrWhiteSpace(CustomSql);
        }
    }
}
