using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseConstraints
    {
        /*
         * View this page for column constraint references:
         * https://docs.liquibase.com/change-types/nested-tags/column.html
         */

        [JsonPropertyName("checkConstraint")]
        public string? CheckConstraint { get; set; }

        [JsonPropertyName("deleteCascade")]
        public bool? DeleteCascade { get; set; }

        [JsonPropertyName("deferrable")]
        public bool? Deferrable { get; set; }

        [JsonPropertyName("foreignKeyName")]
        public string? ForeignKeyName { get; set; }

        [JsonPropertyName("initiallyDeferred")]
        public bool? InitiallyDeferred { get; set; }

        [JsonPropertyName("notNullConstraintName")]
        public string? NotNullConstraintName { get; set; }

        [JsonPropertyName("nullable")]
        public bool? Nullable { get; set; }

        [JsonPropertyName("primaryKey")]
        public bool? PrimaryKey { get; set; }

        [JsonPropertyName("primaryKeyName")]
        public string? PrimaryKeyName { get; set; }

        [JsonPropertyName("primaryKeyTablespace")]
        public string? PrimaryKeyTablespace { get; set; }

        [JsonPropertyName("unique")]
        public bool? Unique { get; set; }

        [JsonPropertyName("uniqueConstraintName")]
        public string? UniqueConstraintName { get; set; }

        [JsonPropertyName("references")]
        public string? References { get; set; }

        [JsonPropertyName("referencedColumnNames")]
        public string? ReferencedColumnNames { get; set; }

        [JsonPropertyName("referencedTableName")]
        public string? ReferencedTableName { get; set; }

        [JsonPropertyName("referencedTableSchemaName")]
        public string? ReferencedTableSchemaName { get; set; }

        [JsonPropertyName("validateForeignKey")]
        public bool? ValidateForeignKey { get; set; }

        [JsonPropertyName("validateNullable")]
        public bool? ValidateNullable { get; set; }

        [JsonPropertyName("validatePrimaryKey")]
        public bool? ValidatePrimaryKey { get; set; }

        [JsonPropertyName("validateUnique")]
        public bool? ValidateUnique { get; set; }
    }
}
