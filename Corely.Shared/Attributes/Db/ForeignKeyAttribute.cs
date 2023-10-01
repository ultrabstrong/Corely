using Corely.Shared.Extensions;

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
            Table = table
                .ThrowIfNullOrWhiteSpace(nameof(table));

            Columns = columns
                .ThrowIfAnyNullOrWhiteSpace(nameof(columns));
        }

        public ForeignKeyAttribute(string schema, string table, params string[] columns)
            : this(table, columns)
        {
            Schema = schema
                .ThrowIfNullOrWhiteSpace(nameof(schema));
        }

        public ForeignKeyAttribute(string customSql)
        {
            CustomSql = customSql
                .ThrowIfNullOrWhiteSpace(nameof(customSql));
        }

        public bool IsCustom()
        {
            return !string.IsNullOrWhiteSpace(CustomSql);
        }
    }
}
