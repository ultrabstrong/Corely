using Corely.Security.Encryption.Providers;

namespace Corely.Security.Encryption.Models
{
    public class SymmetricEncryptedValue : ISymmetricEncryptedValue
    {
        private readonly object _lock = new();
        private readonly ISymmetricEncryptionProvider _encryptionProvider;

        public SymmetricEncryptedValue(ISymmetricEncryptionProvider encryptionProvider)
        {
            ArgumentNullException.ThrowIfNull(encryptionProvider, nameof(encryptionProvider));
            _encryptionProvider = encryptionProvider;
        }

        public string Secret
        {
            get => _secret;
            init => _secret = value;
        }
        private string _secret = string.Empty;

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
