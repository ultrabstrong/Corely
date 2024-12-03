using Corely.IAM;
using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore;

namespace Corely.DevTools;

internal class SecurityConfigurationProvider : ISecurityConfigurationProvider
{
    private readonly string _symmetricKey;

    public SecurityConfigurationProvider()
    {
        var encryptionProvider = new AesEncryptionProvider();
        _symmetricKey = encryptionProvider.GetSymmetricKeyProvider().CreateKey();
    }

    public ISymmetricKeyStoreProvider GetSystemSymmetricKey()
    {
        return new InMemorySymmetricKeyStoreProvider(_symmetricKey);
    }
}
