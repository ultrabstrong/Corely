using Corely.Domain.Entities.Users;

namespace Corely.Domain.Entities.Auth
{
    public class BasicAuthEntity
    {
        public UserEntity User { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
