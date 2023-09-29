namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RemarksAttribute : Attribute
    {
        public string Remarks { get; }

        public RemarksAttribute(string remarks)
        {
            Remarks = remarks;
        }
    }
}
