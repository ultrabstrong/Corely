using Corely.Domain.Entities.Users;

namespace Corely.Domain.Entities.Accounts
{
    public class AccountEntity
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public virtual ICollection<UserEntity> Users { get; init; } = null!;
    }
}
