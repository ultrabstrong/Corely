namespace Corely.Shared.Attributes.Db
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RemarksAttribute : Attribute
    {
        public string Remarks { get; init; }

        public RemarksAttribute(string remarks)
        {
            Remarks = remarks;
        }
    }
}
