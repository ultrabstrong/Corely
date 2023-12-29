using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Encryption;

namespace Corely.Common.Models.Security
{
    public class EncryptedValue(
        IEncryptionProvider encryptionProvider,
        string secret)
        : IEncryptedValue
    {
        private readonly object _lock = new();
        private readonly IEncryptionProvider _encryptionProvider = encryptionProvider.ThrowIfNull(nameof(encryptionProvider));
        public string Secret { get; private set; } = secret;

        public EncryptedValue(IEncryptionProvider encryptionProvider)
            : this(encryptionProvider, "") { }

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

        public void ReEncrypt()
        {
            Secret = _encryptionProvider.ReEncrypt(Secret);
        }
    }
}
