namespace Corely.IAM.Users.Entities;

public class UserSymmetricKeyEntity : Corely.IAM.Security.Entities.SymmetricKeyEntity
{
    public int UserId { get; set; }
}