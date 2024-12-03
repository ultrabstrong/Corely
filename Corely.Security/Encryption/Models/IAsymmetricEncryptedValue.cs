using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Models;

public interface IAsymmetricEncryptedValue
{
    string Secret { get; }
    void Set(string decryptedValue, IAsymmetricKeyStoreProvider provider);
    string GetDecrypted(IAsymmetricKeyStoreProvider provider);
    void ReEncrypt(IAsymmetricKeyStoreProvider provider);
}
