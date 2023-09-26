using Corely.DevTools.Attributes;
using Corely.Shared.Mappers.Liquibase;

namespace Corely.DevTools.Commands
{
    internal partial class Liquibase : CommandBase
    {
        internal class LiquibaseMapper : CommandBase
        {
            [Argument("Assembly path")]
            private string AssemblyPath { get; init; }

            [Argument("Root entity namespace")]
            private string RootEntityNamespace { get; init; }

            [Option("-t", "--type", Description = "Output type [json|xml]")]
            private string OutputType { get; init; } = "json";

            [Option("-o", "--out", Description = "Output path")]
            private string OutPath { get; init; }

            public LiquibaseMapper() : base("map", "Liquibase mapper operations")
            {
            }

            public override void Execute()
            {
                IEntityToLiquibaseMapper mapper = GetMapper();
                var createTableJson = mapper.MapEntitiesInNamespace(RootEntityNamespace);
                if (!string.IsNullOrEmpty(OutPath))
                {
                    OutputToFile(createTableJson);
                }
                else
                {
                    Console.WriteLine(createTableJson);
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

            private void OutputToFile(string createTableJson)
            {
                FileInfo info = new(OutPath);
                if (!info.Directory?.Exists ?? false)
                {
                    Error($"Directory {info.Directory?.FullName} does not exist");
                }
                else if (info.Exists)
                {
                    Error($"File {info.FullName} already exists");
                }
                else
                {
                    File.WriteAllText(OutPath, createTableJson);
                }
            }
        }
    }
}
