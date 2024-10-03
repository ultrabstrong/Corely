using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Providers
{
    public interface IAsymmetricEncryptionProvider
    {
        string EncryptionTypeCode { get; }
        string Encrypt(string value, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider);
        string Decrypt(string value, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider);
        string ReEncrypt(string value, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider);
        string? RemoveEncodedEncryptionData(string value);
    }
}
