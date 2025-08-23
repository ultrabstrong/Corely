namespace Corely.IAM.Users.Entities;

public class UserAsymmetricKeyEntity : Corely.IAM.Security.Entities.AsymmetricKeyEntity
{
    public int UserId { get; set; }
}