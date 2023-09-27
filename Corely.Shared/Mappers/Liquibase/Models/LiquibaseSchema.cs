namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseSchema
    {
        public List<LiquibaseDatabaseChangeLog> DatabaseChangeLog { get; set; }
    }
}
