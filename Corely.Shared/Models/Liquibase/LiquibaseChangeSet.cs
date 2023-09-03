namespace Corely.Shared.Models.Liquibase
{
    public class LiquibaseChangeSet
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public List<LiquibaseChange> Changes { get; set; }
    }
}
