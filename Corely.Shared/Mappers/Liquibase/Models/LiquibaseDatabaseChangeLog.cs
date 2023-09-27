namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseDatabaseChangeLog
    {
        public List<LiquibaseChangeSet> ChangeSets { get; set; }
    }
}
