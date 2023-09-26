namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncrementAttribute : Attribute
    {
        public AutoIncrementAttribute()
        {
        }

        public AutoIncrementAttribute(int startWith, int incrementBy)
            : this(incrementBy)
        {
            StartWith = startWith;
        }

        public AutoIncrementAttribute(int incrementBy)
        {
            IncrementBy = incrementBy;
        }

        public int? StartWith { get; init; }
        public int? IncrementBy { get; init; }
    }
}
