namespace Corely.DevTools.Commands
{
    internal partial class Liquibase : CommandBase
    {
        public Liquibase() : base("lb", "Liquibase operations")
        {
        }

        public override void Execute()
        {
            Console.WriteLine("Please provide one of the following sub-commands");
        }
    }
}
