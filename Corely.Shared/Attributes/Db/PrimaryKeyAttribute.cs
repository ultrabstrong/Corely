namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public string? Tablespace { get; init; }

        public bool? Validate { get; init; }
    }
}
