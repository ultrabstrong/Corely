﻿using Corely.DevTools.Commands;
using System.CommandLine;
using System.Reflection;

namespace Corely.DevTools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var rootCommand = new RootCommand();
                foreach (var command in GetCommands())
                {
                    rootCommand.AddCommand(command);
                }
                rootCommand.InvokeAsync(args).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception caught:{Environment.NewLine}{ex}");
            }
        }

        static List<CommandBase> GetCommands()
        {
            var commandInstances = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                    type.Namespace == "Corely.DevTools.Commands" &&
                    type.IsSubclassOf(typeof(CommandBase)))
                .Select(type => Activator.CreateInstance(type) as CommandBase)
                .Where(commandInstances => commandInstances != null)
                .ToList();

            foreach (var command in commandInstances)
            {
                AddSubCommands(command);
            }

#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            // For some reason it doesn't realize that there is a Where linq statement above that filters out nulls
            return commandInstances;
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        static void AddSubCommands(CommandBase command)
        {
            var subCommandInstances = command
                .GetType()
                .GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
                .Where(type => type.IsSubclassOf(typeof(CommandBase)))
                .Select(type => Activator.CreateInstance(type) as CommandBase)
                .Where(commandInstances => commandInstances != null)
                .ToList();

            foreach (var subCommand in subCommandInstances)
            {
                AddSubCommands(subCommand);
                command.AddCommand(subCommand);
            }
        }
    }
}