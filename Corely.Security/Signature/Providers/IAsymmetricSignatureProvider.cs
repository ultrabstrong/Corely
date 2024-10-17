using Corely.Security.Keys;
using Corely.Security.KeyStore;
using Microsoft.IdentityModel.Tokens;

namespace Corely.Security.Signature.Providers
{
    public interface IAsymmetricSignatureProvider
    {
        string SignatureTypeCode { get; }
        IAsymmetricKeyProvider GetAsymmetricKeyProvider();
        string Sign(string data, IAsymmetricKeyStoreProvider keyStoreProvider);
        bool Verify(string data, string signature, IAsymmetricKeyStoreProvider keyStoreProvider);
        string? RemoveEncodedSignatureData(string value);
        SigningCredentials GetSigningCredentials(string privateKey);
    }
}
