using Corely.Security.KeyStore;

namespace Corely.Security.Signature.Providers
{
    public interface ISignatureProvider
    {
        string SignatureTypeCode { get; }
        string Sign(string data, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider);
        bool Verify(string data, string signature, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider);
    }
}
