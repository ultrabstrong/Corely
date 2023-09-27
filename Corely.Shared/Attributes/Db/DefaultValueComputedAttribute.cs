namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultValueComputedAttribute : Attribute
    {
        public string Expression { get; init; }

        public DefaultValueComputedAttribute(string expression)
        {
            Expression = expression;
        }
    }
}
