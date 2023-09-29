using Corely.DevTools.Attributes;
using Corely.Shared.Mappers.Liquibase;
using Corely.Shared.Mappers.Liquibase.EntityMappers;

namespace Corely.DevTools.Commands
{
    internal partial class Liquibase : CommandBase
    {
        internal partial class LiquibaseMapper : CommandBase
        {
            internal class LiquibaseEntityMapper : CommandBase
            {
                [Argument("Assembly path")]
                private string AssemblyPath { get; init; }

                [Argument("Root entity namespace")]
                private string RootEntityNamespace { get; init; }

                [Option("-t", "--type", Description = "Output type [json|xml]")]
                private string OutputType { get; init; } = "json";

                [Option("-o", "--out", Description = "Output path")]
                private string OutPath { get; init; }

                [Option("-i", "--id", Description = "Changeset id")]
                private string Id { get; init; }

                [Option("-a", "--author", Description = "Changeset author")]
                private string Author { get; init; }

                public LiquibaseEntityMapper() : base("entity", "Liquibase entity mapper operations")
                {
                }

                public override void Execute()
                {
                    ILiquibaseMapper mapper = GetMapper();
                    var createTableJson = mapper.Map();
                    if (!string.IsNullOrEmpty(OutPath))
                    {
                        OutputToFile(createTableJson);
                    }
                    else
                    {
                        Console.WriteLine(createTableJson);
                    }
                }

                private ILiquibaseMapper GetMapper()
                {
                    return OutputType switch
                    {
                        "json" => new EntityToLiquibaseJsonMapper(AssemblyPath, RootEntityNamespace) { Id = Id, Author = Author },
                        "xml" => new EntityToLiquibaseXmlMapper(AssemblyPath, RootEntityNamespace) { Id = Id, Author = Author },
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
}
