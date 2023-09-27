namespace Corely.Shared.Mappers.Liquibase.Models
{
    public sealed class LiquibaseCreateTable
    {
        public string TableName { get; set; }
        public List<LiquibaseColumn> Columns { get; set; }
    }
}
