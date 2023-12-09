using Corely.Shared.Extensions;

namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        public string ReferencedTable { get; }

        public string[] ReferencedColumns { get; }

        public string? ReferencedSchema { get; }

        public string? CustomSql { get; }

        public bool? Validate { get; init; }

        public ForeignKeyAttribute(string table, params string[] columns)
        {
            ReferencedTable = table
                .ThrowIfNullOrWhiteSpace(nameof(table));

            ReferencedColumns = columns
                .ThrowIfAnyNullOrWhiteSpace(nameof(columns));
        }

        public ForeignKeyAttribute(string schema, string table, params string[] columns)
            : this(table, columns)
        {
            ReferencedSchema = schema
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
