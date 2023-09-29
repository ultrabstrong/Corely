namespace Corely.DevTools.Commands
{
    internal partial class Liquibase : CommandBase
    {
        internal partial class LiquibaseMapper : CommandBase
        {
            public LiquibaseMapper() : base("map", "Liquibase map operations")
            {
            }

            public override void Execute()
            {
                ShowHelp("You must specify a subcommand");
            }
        }
    }
}
