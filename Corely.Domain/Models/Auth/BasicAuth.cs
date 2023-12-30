using Corely.Common.Models.Security;

namespace Corely.Domain.Models.Auth
{
    public class BasicAuth
    {
        public string Username { get; init; } = null!;
        public IHashedValue Password { get; init; } = null!;
        public DateTime ModifiedUtc { get; init; }
    }
}
