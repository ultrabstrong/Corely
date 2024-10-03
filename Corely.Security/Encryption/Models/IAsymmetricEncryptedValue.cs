using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Models
{
    public interface IAsymmetricEncryptedValue
    {
        string Secret { get; }
        void Set(string decryptedValue, IAsymmetricEncryptionKeyStoreProvider provider);
        string GetDecrypted(IAsymmetricEncryptionKeyStoreProvider provider);
        void ReEncrypt(IAsymmetricEncryptionKeyStoreProvider provider);
    }
}
