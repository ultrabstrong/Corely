using Corely.Common.Providers.Redaction;
using Corely.DevTools.Commands;
using Serilog;
using System.CommandLine;
using System.Reflection;

namespace Corely.DevTools
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "ConsoleTest")
                .Enrich.WithProperty("CorrelationId", Guid.NewGuid())
                .Enrich.With(new RedactionEnricher([
                    new PasswordRedactionProvider()]))
                .MinimumLevel.Debug()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

            try
            {
                var rootCommand = new RootCommand();
                foreach (var command in GetCommands())
                {
                    if (command != null)
                    {
                        rootCommand.AddCommand(command);
                    }
                }
                await rootCommand.InvokeAsync(args);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred");
            }
            Log.CloseAndFlush();
            Log.Logger.Information("Program finished.");
        }

        static List<CommandBase?> GetCommands()
        {
            var commandInstances = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                    !type.IsNested &&
                    type.Namespace != null &&
                    type.Namespace.StartsWith("Corely.DevTools.Commands") &&
                    type.IsSubclassOf(typeof(CommandBase)))
                .Select(type => Activator.CreateInstance(type) as CommandBase)
                .ToList();

            foreach (var command in commandInstances)
            {
                if (command != null)
                {
                    AddSubCommands(command);
                }
            }

            return commandInstances;
        }

        static void AddSubCommands(CommandBase command)
        {
            var subCommandInstances = command
                .GetType()
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(type => type.IsSubclassOf(typeof(CommandBase)))
                .Select(type => Activator.CreateInstance(type) as CommandBase)
                .ToList();

            foreach (var subCommand in subCommandInstances)
            {
                if (subCommand != null)
                {
                    AddSubCommands(subCommand);
                    command.AddCommand(subCommand);
                }
            }
        }
    }
}