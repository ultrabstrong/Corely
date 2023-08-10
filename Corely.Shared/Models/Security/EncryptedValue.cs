using Corely.Shared.Providers.Security;

namespace Corely.Shared.Models.Security
{
    public class EncryptedValue : IEncryptedValue
    {
        private readonly object _lock = new();
        private readonly IEncryptionProvider _encryptionProvider;
        public string Secret { get; private set; }

        public EncryptedValue(IEncryptionProvider encryptionProvider, string secret)
        {
            Secret = secret;
            _encryptionProvider = encryptionProvider;
        }

        public void Set(string decryptedValue)
        {
            var encryptedValue = _encryptionProvider.Encrypt(decryptedValue);
            lock (_lock)
            {
                Secret = encryptedValue;
            }
        }

        public string Get()
        {
            return _encryptionProvider.Decrypt(Secret);
        }
    }
}
