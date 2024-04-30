using Corely.Security.KeyStore.Symmetric;

namespace Corely.IAM
{
    public interface ISecurityConfigurationProvider
    {
        public ISymmetricKeyStoreProvider GetSystemSymmetricKey();
    }
}
