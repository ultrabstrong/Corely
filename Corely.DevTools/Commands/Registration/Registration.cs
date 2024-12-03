namespace Corely.DevTools.Commands.Registration;

internal partial class Registration : CommandBase
{
    public Registration() : base("register", "Register operations")
    {
    }

    protected override void Execute()
    {
        Console.WriteLine("You must specify a sub command. Use --help to see the available sub commands");
    }
}
