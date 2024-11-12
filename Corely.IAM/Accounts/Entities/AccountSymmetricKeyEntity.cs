using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Security.Entities;

namespace Corely.IAM.Accounts.Entities
{
    internal class AccountSymmetricKeyEntity : SymmetricKeyEntity, IHasIdPk
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
    }
}
