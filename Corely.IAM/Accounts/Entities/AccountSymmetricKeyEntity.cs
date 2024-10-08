using Corely.IAM.Security.Entities;

namespace Corely.IAM.Accounts.Entities
{
    public class AccountSymmetricKeyEntity : SymmetricKeyEntity
    {
        public int AccountId { get; set; }
    }
}
