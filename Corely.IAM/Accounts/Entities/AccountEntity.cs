using Corely.IAM.Entities;
using Corely.IAM.Security.Models;
using Corely.IAM.Users.Entities;

namespace Corely.IAM.Accounts.Entities
{
    public class AccountEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string AccountName { get; init; } = null!;
        public virtual ICollection<UserEntity> Users { get; init; } = null!;
        public virtual SymmetricKey SymmetricKey { get; init; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
