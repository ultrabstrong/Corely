using Corely.Security.Hashing.Models;

namespace Corely.IAM.Models.Auth
{
    public class BasicAuth
    {
        public int Id { get; init; }
        public int UserId { get; set; }
        public IHashedValue Password { get; set; } = null!;
        public DateTime ModifiedUtc { get; init; }
    }
}
