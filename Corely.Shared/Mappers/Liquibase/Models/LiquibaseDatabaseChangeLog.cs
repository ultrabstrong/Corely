using System.Text.Json.Serialization;

namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseDatabaseChangeLog
    {
        [JsonPropertyName("changeSet")]
        public IEnumerable<LiquibaseChangeSet> ChangeSets { get; set; }
    }
}
