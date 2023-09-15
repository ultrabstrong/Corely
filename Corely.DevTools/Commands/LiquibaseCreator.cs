using Corely.DevTools.Attributes;

namespace Corely.DevTools.Commands
{
    internal partial class Liquibase : CommandBase
    {
        internal class LiquibaseCreator : CommandBase
        {

            [Argument("Connection string")]
            private string ConnectionString { get; init; }

            [Argument("Creation specification file path")]
            private string CreationSpecFilePath { get; init; }

            public LiquibaseCreator() : base("create", "Liquibase creation operations")
            {
            }

            public override void Execute()
            {
                var creationSpec = File.ReadAllText(CreationSpecFilePath);
                Console.WriteLine("Creating database from spec file...");
            }
        }
    }
}
