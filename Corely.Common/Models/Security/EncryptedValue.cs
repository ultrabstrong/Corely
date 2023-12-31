using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Encryption;

namespace Corely.Common.Models.Security
{
    public class EncryptedValue : IEncryptedValue
    {
        private readonly object _lock = new();
        private readonly IEncryptionProvider _encryptionProvider;

        public EncryptedValue(IEncryptionProvider encryptionProvider)
        {
            _encryptionProvider = encryptionProvider.ThrowIfNull(nameof(encryptionProvider));
        }

        public string Secret
        {
            get => _secret;
            init => _secret = value;
        }
        private string _secret = "";

        public void Set(string decryptedValue)
        {
            var encryptedValue = _encryptionProvider.Encrypt(decryptedValue);
            lock (_lock)
            {
                _secret = encryptedValue;
            }
        }

        public string Get()
        {
            return _encryptionProvider.Decrypt(Secret);
        }

        public void ReEncrypt()
        {
            _secret = _encryptionProvider.ReEncrypt(Secret);
        }
    }
}
