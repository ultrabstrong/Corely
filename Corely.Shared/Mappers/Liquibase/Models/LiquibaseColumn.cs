namespace Corely.Shared.Mappers.Liquibase.Models
{
    public class LiquibaseColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public LiquibaseConstraints Constraints { get; set; }
    }
}
