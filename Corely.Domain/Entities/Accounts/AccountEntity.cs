using Corely.Domain.Entities.Users;

namespace Corely.Domain.Entities.Accounts
{
    public class AccountEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string AccountName { get; init; } = null!;
        public virtual ICollection<UserEntity> Users { get; init; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
    }
}
