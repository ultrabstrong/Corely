using Corely.Common.Extensions;
using Corely.IAM.Security.Models;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;

namespace Corely.IAM.Security.Services
{
    internal class SecurityService : ISecurityService
    {
        private readonly ISecurityConfigurationProvider _securityConfigurationProvider;
        private readonly ISymmetricKeyProvider _symmetricKeyProvider;
        private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;

        public SecurityService(
            ISecurityConfigurationProvider securityConfigurationProvider,
            ISymmetricKeyProvider symmetricKeyProvider,
            ISymmetricEncryptionProviderFactory symmetricEncryptionProviderFactory)
        {
            _securityConfigurationProvider = securityConfigurationProvider.ThrowIfNull(nameof(securityConfigurationProvider));
            _symmetricKeyProvider = symmetricKeyProvider.ThrowIfNull(nameof(symmetricKeyProvider));
            _symmetricEncryptionProvider = symmetricEncryptionProviderFactory
                .ThrowIfNull(nameof(symmetricEncryptionProviderFactory))
                .GetDefaultProvider();
        }

        public SymmetricKey GetSymmetricKeyEncryptedWithSystemKey()
        {
            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            var decryptedKey = _symmetricKeyProvider.CreateKey();
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
    }
}
