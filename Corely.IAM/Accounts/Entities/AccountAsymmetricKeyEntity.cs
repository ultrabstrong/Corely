namespace Corely.IAM.Accounts.Entities;

public class AccountAsymmetricKeyEntity : Corely.IAM.Security.Entities.AsymmetricKeyEntity
{
    public int AccountId { get; set; }
}