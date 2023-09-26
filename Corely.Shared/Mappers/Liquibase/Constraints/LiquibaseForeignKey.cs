namespace Corely.Shared.Mappers.Liquibase.Constraints
{
    public class LiquibaseForeignKey
    {
        public string Table { get; init; }

        public string Column { get; init; }

        public string GetName()
        {
            return $"fk_{Table}_{Column}";
        }

        public string GetReference()
        {
            return $"{Table}({Column})";
        }
    }
}
