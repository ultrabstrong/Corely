using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Entities;

namespace Corely.IAM.Users.Entities
{
    public class UserEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Enabled { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
        public int FailedLoginsSinceLastSuccess { get; set; }
        public DateTime? LastFailedLoginUtc { get; set; }
        public DateTime? LastLoginUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public virtual UserDetailsEntity? Details { get; set; }
        public virtual BasicAuthEntity? BasicAuth { get; set; }
        public virtual ICollection<AccountEntity>? Accounts { get; set; }
    }
}
