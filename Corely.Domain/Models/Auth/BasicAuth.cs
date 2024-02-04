using Corely.Common.Models.Security;

namespace Corely.Domain.Models.Auth
{
    public class BasicAuth
    {
        public int Id { get; init; }
        public int UserId { get; set; }
        public string Username { get; init; } = null!;
        public IHashedValue Password { get; set; } = null!;
        public DateTime ModifiedUtc { get; init; }
    }
}
