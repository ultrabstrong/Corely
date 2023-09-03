namespace Corely.Shared.Models.Liquibase
{
    public class LiquibaseCreateTable
    {
        public string TableName { get; set; }
        public List<LiquibaseColumn> Columns { get; set; }
    }
}
