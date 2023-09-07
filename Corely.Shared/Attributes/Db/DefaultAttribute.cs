namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {
        public string Value { get; set; }
        public DefaultAttribute(string value) => Value = value;
    }
}
