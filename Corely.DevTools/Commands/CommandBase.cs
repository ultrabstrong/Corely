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

        protected CommandBase(string name, string description, string additionalDescription)
            : this(name, $"{description}{Environment.NewLine}{additionalDescription}")
        { }

        protected CommandBase(string name, string description) : base(name, description)
        {
            var type = GetType();
            foreach (var property in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var optionAttribute = property.GetCustomAttribute<OptionAttribute>();
                if (optionAttribute == null)
                {
                    var argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>();
                    if (CreateArgument(property, argumentAttribute, out var argument))
                    {
                        _arguments.Add(property.Name, argument);
                        AddArgument(argument);
                    }
                }
                else
                {
                    if (CreateOption(property, optionAttribute, out var option))
                    {
                        _options.Add(property.Name, option);
                        AddOption(option);
                    }
                }
            }

            Handler = CommandHandler.Create(InvokeExecute);
        }

        private bool CreateArgument(PropertyInfo property, ArgumentAttribute? argumentAttribute, out Argument argument)
        {
            var argumentGenericType = typeof(Argument<>).MakeGenericType(property.PropertyType);
            var optionalText = argumentAttribute?.IsRequired ?? false ? "" : "[Optional] ";

            var argumentInstance = Activator.CreateInstance(
                argumentGenericType,
                new object[]
                {
                    property.Name,
                    $"{optionalText}{argumentAttribute?.Description}"});

            if (argumentInstance is Argument arg)
            {
                if (argumentAttribute != null)
                {
                    if (argumentAttribute.ArgumentArity != null)
                    {
                        arg.Arity = argumentAttribute.ArgumentArity.Value;
                    }
                    if (argumentAttribute.IsRequired)
                    {
                        arg.SetDefaultValue(property.GetValue(this));
                    }
                }

                argument = arg;
                return true;
            }

            argument = null;
            return false;
        }

        private bool CreateOption(PropertyInfo property, OptionAttribute optionAttribute, out Option option)
        {
            var optionGenericType = typeof(Option<>).MakeGenericType(property.PropertyType);
            var optionInstance = Activator.CreateInstance(
                optionGenericType,
                new object[] {
                    optionAttribute.Aliases,
                    optionAttribute.Description});

            if (optionInstance is Option opt)
            {
                if (optionAttribute.ArgumentArity != null)
                {
                    opt.Arity = optionAttribute.ArgumentArity.Value;
                }
                opt.SetDefaultValue(property.GetValue(this));

                option = opt;
                return true;
            }

            option = null;
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
