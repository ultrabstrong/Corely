using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseChange
    {
        [JsonPropertyName("createTable")]
        public LiquibaseCreateTable CreateTable { get; set; }
    }
}
