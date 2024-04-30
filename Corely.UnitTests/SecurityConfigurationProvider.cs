using Corely.IAM;
using Corely.Security.Keys.Symmetric;
using Corely.Security.KeyStore.Symmetric;

namespace Corely.UnitTests
{
    internal class SecurityConfigurationProvider : ISecurityConfigurationProvider
    {
        public ISymmetricKeyStoreProvider GetSystemSymmetricKey()
        {
            var key = new AesKeyProvider().CreateKey();
            return new InMemorySymmetricKeyStoreProvider(key);
        }
    }
}
