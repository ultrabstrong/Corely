using Corely.Common.Models.Security;

namespace Corely.Domain.Models.Auth
{
    public class BasicAuth
    {
        public string Username { get; init; }
        public IHashedValue Password { get; init; }
        public DateTime ModifiedUtc { get; init; }
    }
}
