namespace Corely.DevTools.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class OptionAttribute(
        params string[] aliases)
        : AttributeBase
    {
        public string[] Aliases { get; init; } = aliases;
    }
}
