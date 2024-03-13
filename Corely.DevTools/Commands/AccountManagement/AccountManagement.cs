namespace Corely.DevTools.Commands.AccountManagement
{
    internal partial class AccountManagement : CommandBase
    {
        public AccountManagement() : base("account", "Account management operations")
        {
        }

        protected override void Execute()
        {
            Console.WriteLine("You must specify a sub command. Use --help to see the available sub commands");
        }
    }
}
