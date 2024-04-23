using Corely.Security.Encryption.Providers;

namespace Corely.Security.Encryption.Models
{
    public class EncryptedValue : IEncryptedValue
    {
        private readonly object _lock = new();
        private readonly IEncryptionProvider _encryptionProvider;

        public EncryptedValue(IEncryptionProvider encryptionProvider)
        {
            ArgumentNullException.ThrowIfNull(encryptionProvider, nameof(encryptionProvider));
            _encryptionProvider = encryptionProvider;
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

        public string GetDecrypted()
        {
            return _encryptionProvider.Decrypt(Secret);
        }

        public void ReEncrypt()
        {
            _secret = _encryptionProvider.ReEncrypt(Secret);
        }
    }
}
