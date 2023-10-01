using Corely.Shared.Extensions;

namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultValueComputedAttribute : Attribute
    {
        public string Expression { get; }

        public DefaultValueComputedAttribute(string expression)
        {
            Expression = expression
                .ThrowIfNullOrWhiteSpace(nameof(expression));
        }
    }
}
