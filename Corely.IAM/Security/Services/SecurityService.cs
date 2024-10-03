using Corely.Common.Extensions;
using Corely.IAM.Security.Models;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;

namespace Corely.IAM.Security.Services
{
    internal class SecurityService : ISecurityService
    {
        private readonly ISecurityConfigurationProvider _securityConfigurationProvider;
        private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
        private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;

        public SecurityService(
            ISecurityConfigurationProvider securityConfigurationProvider,
            ISymmetricEncryptionProviderFactory symmetricEncryptionProviderFactory,
            IAsymmetricEncryptionProviderFactory asymmetricEncryptionProviderFactory)
        {
            _securityConfigurationProvider = securityConfigurationProvider.ThrowIfNull(nameof(securityConfigurationProvider));

            _symmetricEncryptionProvider = symmetricEncryptionProviderFactory
                .ThrowIfNull(nameof(symmetricEncryptionProviderFactory))
                .GetDefaultProvider();

            _asymmetricEncryptionProvider = asymmetricEncryptionProviderFactory
                .ThrowIfNull(nameof(asymmetricEncryptionProviderFactory))
                .GetDefaultProvider();
        }

        public SymmetricKey GetSymmetricKeyEncryptedWithSystemKey()
        {
            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            var decryptedKey = _symmetricEncryptionProvider.GetSymmetricKeyProvider().CreateKey();
            var encryptedKey = _symmetricEncryptionProvider.Encrypt(decryptedKey, systemKeyStoreProvider);
            var symmetricKey = new SymmetricKey
            {
                Version = systemKeyStoreProvider.GetCurrentVersion(),
                Key = new SymmetricEncryptedValue(_symmetricEncryptionProvider)
                {
                    Secret = encryptedKey
                }
            };
            return symmetricKey;
        }

        public AsymmetricKey GetAsymmetricKeyEncryptedWithSystemKey()
        {
            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            var (publickey, privateKey) = _asymmetricEncryptionProvider.GetAsymmetricKeyProvider().CreateKeys();
            var encryptedPrivateKey = _symmetricEncryptionProvider.Encrypt(privateKey, systemKeyStoreProvider);
            var asymmetricKey = new AsymmetricKey
            {
                Version = systemKeyStoreProvider.GetCurrentVersion(),
                PublicKey = publickey,
                PrivateKey = new SymmetricEncryptedValue(_symmetricEncryptionProvider)
                {
                    Secret = encryptedPrivateKey
                }
            };
            return asymmetricKey;
        }
    }
}
