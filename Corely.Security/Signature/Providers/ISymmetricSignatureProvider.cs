using Corely.Security.Keys;
using Corely.Security.KeyStore;
using Microsoft.IdentityModel.Tokens;

namespace Corely.Security.Signature.Providers;

public interface ISymmetricSignatureProvider
{
    string SignatureTypeCode { get; }
    ISymmetricKeyProvider GetSymmetricKeyProvider();
    string Sign(string data, ISymmetricKeyStoreProvider keyStoreProvider);
    bool Verify(string data, string signature, ISymmetricKeyStoreProvider keyStoreProvider);
    SigningCredentials GetSigningCredentials(string key);
}
