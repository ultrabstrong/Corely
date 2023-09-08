using Corely.DevTools.Attributes;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;


namespace Corely.DevTools.Commands
{
    internal abstract class CommandBase : Command
    {
        private readonly Dictionary<string, Argument> _arguments = new();
        private readonly Dictionary<string, Option> _options = new();

        public CommandBase(string name, string description) : base(name, description)
        {
            var type = GetType();
            foreach (var property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var optionAttribute = property.GetCustomAttribute<OptionAttribute>();
                if (optionAttribute != null)
                {
                    if (CreateOption(property, optionAttribute, out var option))
                    {
                        _options.Add(property.Name, option);
                        AddOption(option);
                    }
                }
                else
                {
                    var argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>();
                    if (CreateArgument(property, argumentAttribute, out var argument))
                    {
                        _arguments.Add(property.Name, argument);
                        AddArgument(argument);
                    }
                }
            }

            Handler = CommandHandler.Create(InvokeExecute);
        }

        private bool CreateOption(PropertyInfo property, OptionAttribute optionAttribute, out Option option)
        {
            var optionGenericType = typeof(Option<>).MakeGenericType(property.PropertyType);
            var optionInstance = Activator.CreateInstance(
                optionGenericType,
                new object[] {
                    optionAttribute.Aliases,
                    optionAttribute.Description });

            if (optionInstance is Option opt)
            {
                opt.SetDefaultValue(property.GetValue(this));
                option = opt;
                return true;
            }

            option = null;
            return false;
        }

        private bool CreateArgument(PropertyInfo property, ArgumentAttribute? argumentAttribute, out Argument argument)
        {
            var argumentGenericType = typeof(Argument<>).MakeGenericType(property.PropertyType);
            var argumentInstance = Activator.CreateInstance(
                argumentGenericType,
                new object[]
                {
                    property.Name,
                    argumentAttribute?.Description ?? ""});

            if (argumentInstance is Argument arg)
            {
                if (argumentAttribute != null) { arg.SetDefaultValue(property.GetValue(this)); }
                argument = arg;
                return true;
            }

            argument = null;
            return false;
        }

        private void InvokeExecute(BindingContext context)
        {
            var type = GetType();
            foreach (var property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var value = _options.TryGetValue(property.Name, out Option? option)
                    ? context.ParseResult.GetValueForOption(option)
                    : context.ParseResult.GetValueForArgument(_arguments[property.Name]);

                if (value != null)
                {
                    property.SetValue(this, value);
                }
            }

            Execute();
        }

        public abstract void Execute();
    }
}
