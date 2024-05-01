using Corely.IAM;
using Corely.Security.Keys.Symmetric;
using Corely.Security.KeyStore.Symmetric;

namespace ConsoleTest
{
    internal class SecurityConfigurationProvider : ISecurityConfigurationProvider
    {
        private readonly string _key;

        public SecurityConfigurationProvider()
        {
            _key = new AesKeyProvider().CreateKey();
        }

        public ISymmetricKeyStoreProvider GetSystemSymmetricKey()
        {
            return new InMemorySymmetricKeyStoreProvider(_key);
        }
    }
}
