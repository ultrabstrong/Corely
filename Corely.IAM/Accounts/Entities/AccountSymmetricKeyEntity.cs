namespace Corely.IAM.Accounts.Entities;

public class AccountSymmetricKeyEntity : Corely.IAM.Security.Entities.SymmetricKeyEntity
{
    public int AccountId { get; set; }
}