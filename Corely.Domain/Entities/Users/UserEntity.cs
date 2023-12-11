using Corely.Domain.Entities.Auth;

namespace Corely.Domain.Entities.Users
{
    public class UserEntity
    {
        public int Id { get; set; }

        public DateTime CreatedUtc { get; set; }

        public bool Enabled { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public virtual UserDetailsEntity? Details { get; set; }

        public virtual BasicAuthEntity? BasicAuth { get; set; }
    }
}
