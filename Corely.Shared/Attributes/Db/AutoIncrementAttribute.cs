namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AutoIncrementAttribute : Attribute
    {
        private int? _startWith;
        private int? _incrementBy;

        public int? StartWith
        {
            get => _startWith;
            init
            {
                if (value.HasValue && value < 0)
                {
                    throw new ArgumentOutOfRangeException("StartWith cannot be less than 0");
                }
                _startWith = value;
            }
        }

        public int? IncrementBy
        {
            get => _incrementBy;
            init
            {
                if (value.HasValue && value < 1)
                {
                    throw new ArgumentOutOfRangeException("IncrementBy cannot be less than 1");
                }
                _incrementBy = value;
            }
        }

        public AutoIncrementAttribute() { }

        public AutoIncrementAttribute(int startWith, int incrementBy)
            : this(incrementBy)
        {
            StartWith = startWith;
        }

        public AutoIncrementAttribute(int incrementBy)
        {
            IncrementBy = incrementBy;
        }
    }
}
