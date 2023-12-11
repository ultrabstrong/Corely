using Corely.Shared.Models.Security;

namespace Corely.Domain.Models.Auth
{
    public class BasicAuth : IValidate
    {
        public string Username { get; init; }
        public IEncryptedValue Password { get; init; }
        public DateTime ModifiedUtc { get; init; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username)
                && Password != null;
        }
    }
}
