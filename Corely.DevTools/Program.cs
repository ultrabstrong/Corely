using Corely.DevTools.Commands;
using System.CommandLine;

namespace Corely.DevTools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var rootCommand = new RootCommand();
                rootCommand.AddCommand(new Add());
                rootCommand.InvokeAsync(args).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught:{Environment.NewLine}{ex}");
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}