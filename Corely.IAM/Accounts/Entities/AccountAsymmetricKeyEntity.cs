using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Security.Entities;

namespace Corely.IAM.Accounts.Entities
{
    public class AccountAsymmetricKeyEntity : AsymmetricKeyEntity, IHasIdPk
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
    }
}
