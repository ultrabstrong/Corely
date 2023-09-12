namespace Corely.DevTools.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class OptionAttribute : Attribute
    {
        public string[] Aliases { get; init; }

        public string Description { get; init; } = null;

        public OptionAttribute(params string[] aliases)
        {
            Aliases = aliases;
        }
    }
}
