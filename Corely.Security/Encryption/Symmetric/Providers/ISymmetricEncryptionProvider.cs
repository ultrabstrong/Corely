using Corely.Security.KeyStore.Symmetric;

namespace Corely.Security.Encryption.Providers
{
    public interface ISymmetricEncryptionProvider
    {
        string EncryptionTypeCode { get; }
        string Encrypt(string value, ISymmetricKeyStoreProvider provider);
        string Decrypt(string value, ISymmetricKeyStoreProvider provider);
        string ReEncrypt(string value, ISymmetricKeyStoreProvider provider);
    }
}
