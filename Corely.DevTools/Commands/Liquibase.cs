using Corely.DevTools.Attributes;
using Corely.Shared.Mappers.Liquibase;

namespace Corely.DevTools.Commands
{
    internal class Liquibase : CommandBase
    {
        [Argument("Assembly path")]
        private string AssemblyPath { get; init; }

        [Argument("Root entity namespace")]
        private string RootEntityNamespace { get; init; }

        [Option("-o", "--output-type", Description = "Output type [json|xml]")]
        private string OutputType { get; init; } = "json";

        [Option("-c", "--create-db", Description = "Create database")]
        private string ConnectionString { get; init; }

        public Liquibase() : base("lb", "Liquibase operations")
        {
        }

        public override void Execute()
        {
            IEntityToLiquibaseMapper mapper = GetMapper();
            var createTableJson = mapper.MapEntitiesInNamespace(RootEntityNamespace);

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "create-tables.json");
            File.WriteAllText(filePath, createTableJson);

            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                //todo: implement liquibase upload
            }
        }

        private IEntityToLiquibaseMapper GetMapper()
        {
            return OutputType switch
            {
                "json" => new EntityToLiquibaseJsonMapper(AssemblyPath),
                "xml" => new EntityToLiquibaseXmlMapper(AssemblyPath),
                _ => throw new NotSupportedException($"Output type {OutputType} is not supported")
            };
        }
    }
}
