namespace Corely.Shared.Models.Liquibase
{
    public class LiquibaseColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public LiquibaseConstraints Constraints { get; set; }
    }
}
