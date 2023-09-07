namespace Corely.Shared.Mappers.Liquibase.Models
{
    public class LiquibaseConstraints
    {
        public bool? PrimaryKey { get; set; }
        public bool? Nullable { get; set; }
        public bool? Unique { get; set; }
        public string? ForeignKey { get; set; }
        public string? ForeignKeyTable { get; set; }
        public string? CheckExpression { get; set; }
        public string? Default { get; set; }
        public bool? AutoIncrement { get; set; }
    }
}
