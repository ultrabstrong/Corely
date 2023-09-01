using Corely.Shared.Models.Security;

namespace Corely.Domain.Models.Auth
{
    internal class BasicAuth
    {
        public string Username { get; set; }
        public IEncryptedValue Password { get; set; }
    }
}
