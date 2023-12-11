using Corely.Domain.Entities.Users;

namespace Corely.Domain.Entities.Auth
{
    public class BasicAuthEntity : CreatedAndModifiedEntity
    {
        public UserEntity User { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
