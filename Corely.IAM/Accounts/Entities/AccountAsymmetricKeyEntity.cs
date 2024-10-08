using Corely.IAM.Security.Entities;

namespace Corely.IAM.Accounts.Entities
{
    public class AccountAsymmetricKeyEntity : AsymmetricKeyEntity
    {
        public int AccountId { get; set; }
    }
}
