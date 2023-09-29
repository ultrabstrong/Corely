using Corely.Shared.Converters.Json;
using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseColumn
    {

        /*
         * View this page for column attribute references:
         * https://docs.liquibase.com/change-types/nested-tags/column.html
         */

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("afterColumn")]
        public string? AfterColumn { get; set; }

        [JsonPropertyName("autoIncrement")]
        public bool? AutoIncrement { get; set; }

        [JsonPropertyName("beforeColumn")]
        public string? BeforeColumn { get; set; }

        [JsonPropertyName("computed")]
        public bool? Computed { get; set; }

        [JsonPropertyName("defaultValue")]
        public object? DefaultValue { get; set; }

        [JsonPropertyName("defaultValueBoolean")]
        public bool? DefaultValueBoolean { get; set; }

        [JsonPropertyName("defaultValueComputed")]
        public string? DefaultValueComputed { get; set; }

        [JsonPropertyName("defaultValueConstraintName")]
        public string? DefaultValueConstraintName { get; set; }

        [JsonPropertyName("defaultValueDate")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? DefaultValueDate { get; set; }

        [JsonPropertyName("defaultValueNumeric")]
        public object? DefaultValueNumeric { get; set; }

        [JsonPropertyName("descending")]
        public bool? Descending { get; set; }

        [JsonPropertyName("encoding")]
        public string? Encoding { get; set; }

        [JsonPropertyName("incrementBy")]
        public int? IncrementBy { get; set; }

        [JsonPropertyName("position")]
        public int? Position { get; set; }

        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

        [JsonPropertyName("startWith")]
        public int? StartWith { get; set; }

        [JsonPropertyName("valueBlobFile")]
        public string? ValueBlobFile { get; set; }

        [JsonPropertyName("valueBoolean")]
        public bool? ValueBoolean { get; set; }

        [JsonPropertyName("valueClobFile")]
        public string? ValueClobFile { get; set; }

        [JsonPropertyName("valueComputed")]
        public string? ValueComputed { get; set; }

        [JsonPropertyName("valueDate")]
        [JsonConverter(typeof(JsonDateTimeConverter))]
        public DateTime? ValueDate { get; set; }

        [JsonPropertyName("valueNumeric")]
        public object? ValueNumeric { get; set; }

        [JsonPropertyName("constraints")]
        public LiquibaseConstraints Constraints { get; set; }
    }
}
