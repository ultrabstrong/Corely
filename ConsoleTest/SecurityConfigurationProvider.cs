using Corely.IAM;
using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace ConsoleTest
{
    internal class SecurityConfigurationProvider : ISecurityConfigurationProvider
    {
        private readonly string _symmetricKey;

        public SecurityConfigurationProvider()
        {
            _symmetricKey = new AesKeyProvider().CreateKey();
        }

        public ISymmetricKeyStoreProvider GetSystemSymmetricKey()
        {
            return new InMemorySymmetricKeyStoreProvider(_symmetricKey);
        }
    }
}
