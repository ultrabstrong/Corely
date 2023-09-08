namespace Corely.DevTools.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class ArgumentAttribute : Attribute
    {
        public string Description { get; } = null;

        public ArgumentAttribute(string description)
        {
            Description = description;
        }
    }
}
