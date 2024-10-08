using Corely.IAM.Entities;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Accounts.Entities
{
    public class AccountEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string AccountName { get; init; } = null!;
        public virtual ICollection<UserEntity>? Users { get; set; }
        public virtual AccountSymmetricKeyEntity? SymmetricKey { get; init; }
        public virtual AccountAsymmetricKeyEntity? AsymmetricKey { get; init; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
