using Corely.Domain.Entities.Users;

namespace Corely.Domain.Entities.Auth
{
    public class BasicAuthEntity : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; } = null!;
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public UserEntity User { get; set; } = null!;
    }
}
