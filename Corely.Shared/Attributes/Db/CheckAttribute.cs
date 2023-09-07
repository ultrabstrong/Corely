namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckAttribute : Attribute
    {
        public string Expression { get; set; }
        public CheckAttribute(string expression) => Expression = expression;
    }
}
