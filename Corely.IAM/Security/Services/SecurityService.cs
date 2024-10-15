using Corely.Common.Extensions;
using Corely.IAM.Security.Enums;
using Corely.IAM.Security.Models;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.Security.Signature.Factories;
using Corely.Security.Signature.Providers;

namespace Corely.IAM.Security.Services
{
    internal class SecurityService : ISecurityService
    {
        private readonly ISecurityConfigurationProvider _securityConfigurationProvider;
        private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
        private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;
        private readonly IAsymmetricSignatureProvider _asymmetricSignatureProvider;

        public SecurityService(
            ISecurityConfigurationProvider securityConfigurationProvider,
            ISymmetricEncryptionProviderFactory symmetricEncryptionProviderFactory,
            IAsymmetricEncryptionProviderFactory asymmetricEncryptionProviderFactory,
            IAsymmetricSignatureProviderFactory asymmetricSignatureProviderFactory)
        {
            _securityConfigurationProvider = securityConfigurationProvider.ThrowIfNull(nameof(securityConfigurationProvider));

            _symmetricEncryptionProvider = symmetricEncryptionProviderFactory
                .ThrowIfNull(nameof(symmetricEncryptionProviderFactory))
                .GetDefaultProvider();

            _asymmetricEncryptionProvider = asymmetricEncryptionProviderFactory
                .ThrowIfNull(nameof(asymmetricEncryptionProviderFactory))
                .GetDefaultProvider();

            _asymmetricSignatureProvider = asymmetricSignatureProviderFactory
                .ThrowIfNull(nameof(asymmetricSignatureProviderFactory))
                .GetDefaultProvider();
        }

        public SymmetricKey GetSymmetricEncryptionKeyEncryptedWithSystemKey()
        {
            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            var decryptedKey = _symmetricEncryptionProvider.GetSymmetricKeyProvider().CreateKey();
            var encryptedKey = _symmetricEncryptionProvider.Encrypt(decryptedKey, systemKeyStoreProvider);
            var symmetricKey = new SymmetricKey
            {
                KeyUsedFor = KeyUsedFor.Encryption,
                Version = systemKeyStoreProvider.GetCurrentVersion(),
                Key = new SymmetricEncryptedValue(_symmetricEncryptionProvider)
                {
                    Secret = encryptedKey
                }
            };
            return symmetricKey;
        }

        public AsymmetricKey GetAsymmetricEncryptionKeyEncryptedWithSystemKey()
        {
            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            var (publickey, privateKey) = _asymmetricEncryptionProvider.GetAsymmetricKeyProvider().CreateKeys();
            var encryptedPrivateKey = _symmetricEncryptionProvider.Encrypt(privateKey, systemKeyStoreProvider);
            var asymmetricKey = new AsymmetricKey
            {
                KeyUsedFor = KeyUsedFor.Encryption,
                Version = systemKeyStoreProvider.GetCurrentVersion(),
                PublicKey = publickey,
                PrivateKey = new SymmetricEncryptedValue(_symmetricEncryptionProvider)
                {
                    Secret = encryptedPrivateKey
                }
            };
            return asymmetricKey;
        }

        public AsymmetricKey GetAsymmetricSignatureKeyEncryptedWithSystemKey()
        {
            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            var (publickey, privateKey) = _asymmetricSignatureProvider.GetAsymmetricKeyProvider().CreateKeys();
            var encryptedPrivateKey = _symmetricEncryptionProvider.Encrypt(privateKey, systemKeyStoreProvider);
            var asymmetricKey = new AsymmetricKey
            {
                KeyUsedFor = KeyUsedFor.Signature,
                Version = systemKeyStoreProvider.GetCurrentVersion(),
                PublicKey = publickey,
                PrivateKey = new SymmetricEncryptedValue(_symmetricEncryptionProvider)
                {
                    Secret = encryptedPrivateKey
                }
            };
            return asymmetricKey;
        }

        public string DecryptWithSystemKey(string encryptedValue)
        {
            if (string.IsNullOrWhiteSpace(encryptedValue))
            {
                return string.Empty;
            }

            var systemKeyStoreProvider = _securityConfigurationProvider.GetSystemSymmetricKey();
            return _symmetricEncryptionProvider.Decrypt(encryptedValue, systemKeyStoreProvider);
        }
    }
}
