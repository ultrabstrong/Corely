using Corely.IAM;
using Corely.IAM.Security.Enums;
using Corely.IAM.Security.Services;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Providers;
using Corely.Security.Signature.Factories;
using Corely.Security.Signature.Providers;

namespace Corely.UnitTests.IAM.Security.Services
{
    public class SecurityServiceTests
    {
        private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
        private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;
        private readonly IAsymmetricSignatureProvider _asymmetricSignatureProvider;
        private readonly SecurityService _securityService;
        public SecurityServiceTests()
        {
            var serviceFactory = new ServiceFactory();

            var symmetricEncryptionProviderFactory = serviceFactory.GetRequiredService<ISymmetricEncryptionProviderFactory>();
            _symmetricEncryptionProvider = symmetricEncryptionProviderFactory.GetDefaultProvider();

            var asymmetricEncryptionProviderFactory = serviceFactory.GetRequiredService<IAsymmetricEncryptionProviderFactory>();
            _asymmetricEncryptionProvider = asymmetricEncryptionProviderFactory.GetDefaultProvider();

            var asymmetricSignatureProviderFactory = serviceFactory.GetRequiredService<IAsymmetricSignatureProviderFactory>();
            _asymmetricSignatureProvider = asymmetricSignatureProviderFactory.GetDefaultProvider();

            _securityService = new(
                serviceFactory.GetRequiredService<ISecurityConfigurationProvider>(),
                symmetricEncryptionProviderFactory,
                asymmetricEncryptionProviderFactory,
                asymmetricSignatureProviderFactory);
        }

        [Fact]
        public void GetSymmetricEncryptionKeyEncryptedWithSystemKey_ReturnsSymmetricKey()
        {
            var result = _securityService.GetSymmetricEncryptionKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.Key);
            Assert.True(result.Version > -1);
            Assert.Equal(KeyUsedFor.Encryption, result.KeyUsedFor);
            Assert.Equal(_symmetricEncryptionProvider.EncryptionTypeCode, result.ProviderTypeCode);

            var decryptedKey = _securityService.DecryptWithSystemKey(result.Key.Secret);

            Assert.True(_symmetricEncryptionProvider
                .GetSymmetricKeyProvider()
                .IsKeyValid(decryptedKey));
        }

        [Fact]
        public void GetAsymmetricEncryptionKeyEncryptedWithSystemKey_ReturnsAsymmetricKey()
        {
            var result = _securityService.GetAsymmetricEncryptionKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.PublicKey);
            Assert.NotNull(result.PrivateKey);
            Assert.True(result.Version > -1);
            Assert.Equal(KeyUsedFor.Encryption, result.KeyUsedFor);
            Assert.Equal(_asymmetricEncryptionProvider.EncryptionTypeCode, result.ProviderTypeCode);

            var decryptedPrivateKey = _securityService.DecryptWithSystemKey(result.PrivateKey.Secret);

            Assert.True(_asymmetricEncryptionProvider
                .GetAsymmetricKeyProvider()
                .IsKeyValid(result.PublicKey, decryptedPrivateKey));
        }

        [Fact]
        public void GetAsymmetricSignatureKeyEncryptedWithSystemKey_ReturnsAsymmetricKey()
        {
            var result = _securityService.GetAsymmetricSignatureKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.PublicKey);
            Assert.NotNull(result.PrivateKey);
            Assert.True(result.Version > -1);
            Assert.Equal(KeyUsedFor.Signature, result.KeyUsedFor);
            Assert.Equal(_asymmetricSignatureProvider.SignatureTypeCode, result.ProviderTypeCode);

            var decryptedPrivateKey = _securityService.DecryptWithSystemKey(result.PrivateKey.Secret);

            Assert.True(_asymmetricSignatureProvider
                .GetAsymmetricKeyProvider()
                .IsKeyValid(result.PublicKey, decryptedPrivateKey));
        }
    }
}
