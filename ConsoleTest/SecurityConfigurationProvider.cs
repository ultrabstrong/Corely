using Corely.IAM;
using Corely.Security.KeyStore.Symmetric;

namespace ConsoleTest
{
    internal class SecurityConfigurationProvider : ISecurityConfigurationProvider
    {
        public ISymmetricKeyStoreProvider GetSystemSymmetricKey()
        {
            var key = ConfigurationProvider.GetSystemKey();
            return new InMemorySymmetricKeyStoreProvider(key);
        }
    }
}
