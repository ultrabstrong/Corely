using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.Security.Signature.Providers
{
    public interface IAsymmetricSignatureProvider
    {
        string SignatureTypeCode { get; }
        IAsymmetricKeyProvider GetAsymmetricKeyProvider();
        string Sign(string data, IAsymmetricKeyStoreProvider keyStoreProvider);
        bool Verify(string data, string signature, IAsymmetricKeyStoreProvider keyStoreProvider);
        string? RemoveEncodedSignatureData(string value);
    }
}
