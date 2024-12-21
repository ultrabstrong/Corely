namespace Corely.IAM.Roles.Models;
public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; } = null!;
    public int AccountId { get; set; }
}
