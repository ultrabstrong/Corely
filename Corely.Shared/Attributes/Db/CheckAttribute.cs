namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CheckAttribute : Attribute
    {
        private bool? _initiallyDeferred;

        public string Expression { get; }

        public bool? Deferrable { get; }

        public bool? InitiallyDeferred
        {
            get => _initiallyDeferred;
            init
            {
                if (value == true && Deferrable != true)
                {
                    throw new ArgumentException("Cannot set InitiallyDeferred to true if Deferrable is not true.");
                }
                _initiallyDeferred = value;
            }
        }

        public CheckAttribute(string expression)
        {
            ArgumentNullException.ThrowIfNull(expression, nameof(expression));
            Expression = expression;
        }

        public CheckAttribute(string expression, bool deferrable)
            : this(expression)
        {
            Deferrable = deferrable;
        }
    }
}
