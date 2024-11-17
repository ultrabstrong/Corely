namespace Corely.IAM.Groups.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; } = null!;
        public int AccountId { get; set; }
    }
}
