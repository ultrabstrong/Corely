using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseChangeSet
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("changes")]
        public IEnumerable<LiquibaseChange> Changes { get; set; }
    }
}
