using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;

namespace Corely.Domain.Entities.Users
{
    public class UserEntity : IHasIdPk, IHasCreatedUtc
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Enabled { get; set; }
        public DateTime CreatedUtc { get; set; }
        public virtual UserDetailsEntity? Details { get; set; }
        public virtual BasicAuthEntity? BasicAuth { get; set; }
        public virtual ICollection<AccountEntity>? Accounts { get; set; }
    }
}
