using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Accounts.Entities
{
    internal class AccountEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string AccountName { get; init; } = null!;
        public virtual ICollection<UserEntity>? Users { get; set; }
        public virtual ICollection<AccountSymmetricKeyEntity>? SymmetricKeys { get; init; }
        public virtual ICollection<AccountAsymmetricKeyEntity>? AsymmetricKeys { get; init; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
