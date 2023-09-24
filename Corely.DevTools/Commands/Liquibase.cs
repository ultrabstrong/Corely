namespace Corely.DevTools.Commands
{
    internal partial class Liquibase : CommandBase
    {
        public Liquibase() : base("lb", "Liquibase operations")
        {
        }

        public override void Execute()
        {
            ShowHelp("You must specify a subcommand");
        }
    }
}
