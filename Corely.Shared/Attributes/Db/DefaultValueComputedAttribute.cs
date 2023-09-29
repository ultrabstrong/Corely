namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultValueComputedAttribute : Attribute
    {
        public string Expression { get; }

        public DefaultValueComputedAttribute(string expression)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(expression, nameof(expression));
            Expression = expression;
        }
    }
}
