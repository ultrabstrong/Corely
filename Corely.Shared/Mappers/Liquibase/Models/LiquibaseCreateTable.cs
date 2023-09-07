namespace Corely.Shared.Mappers.Liquibase.Models
{
    public class LiquibaseCreateTable
    {
        public string TableName { get; set; }
        public List<LiquibaseColumn> Columns { get; set; }
    }
}
