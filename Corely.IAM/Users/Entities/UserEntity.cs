using Corely.DataAccess.Interfaces.Entities;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;

namespace Corely.IAM.Users.Entities
{
    internal class UserEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Disabled { get; set; }
        public int TotalSuccessfulLogins { get; set; }
        public DateTime? LastSuccessfulLoginUtc { get; set; }
        public int FailedLoginsSinceLastSuccess { get; set; }
        public int TotalFailedLogins { get; set; }
        public DateTime? LastFailedLoginUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public virtual ICollection<UserSymmetricKeyEntity>? SymmetricKeys { get; init; }
        public virtual ICollection<UserAsymmetricKeyEntity>? AsymmetricKeys { get; init; }
        public virtual BasicAuthEntity? BasicAuth { get; set; }
        public virtual ICollection<AccountEntity>? Accounts { get; set; }
    }
}
