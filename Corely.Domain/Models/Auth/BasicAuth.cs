using Corely.Shared.Models.Security;

namespace Corely.Domain.Models.Auth
{
    public class BasicAuth
    {
        public string Username { get; init; }
        public IEncryptedValue Password { get; init; }
        public DateTime ModifiedUtc { get; init; }
    }
}
