using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Models
{
    public class AsymmetricEncryptedValue : IAsymmetricEncryptedValue
    {
        private readonly object _lock = new();
        private readonly IAsymmetricEncryptionProvider _encryptionProvider;

        public AsymmetricEncryptedValue(IAsymmetricEncryptionProvider encryptionProvider)
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

        public void Set(string decryptedValue, IAsymmetricEncryptionKeyStoreProvider provider)
        {
            var encryptedValue = _encryptionProvider.Encrypt(decryptedValue, provider);
            lock (_lock)
            {
                _secret = encryptedValue;
            }
        }

        public string GetDecrypted(IAsymmetricEncryptionKeyStoreProvider provider)
        {
            return _encryptionProvider.Decrypt(Secret, provider);
        }

        public void ReEncrypt(IAsymmetricEncryptionKeyStoreProvider provider)
        {
            _secret = _encryptionProvider.ReEncrypt(Secret, provider);
        }
    }
}
