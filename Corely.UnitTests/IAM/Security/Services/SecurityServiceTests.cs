using Corely.IAM;
using Corely.IAM.Security.Services;
using Corely.Security.Encryption.Factories;

namespace Corely.UnitTests.IAM.Security.Services
{
    public class SecurityServiceTests
    {
        private readonly SecurityService _securityService;
        public SecurityServiceTests()
        {
            var serviceFactory = new ServiceFactory();
            _securityService = new(
                serviceFactory.GetRequiredService<ISecurityConfigurationProvider>(),
                serviceFactory.GetRequiredService<ISymmetricEncryptionProviderFactory>(),
                serviceFactory.GetRequiredService<IAsymmetricEncryptionProviderFactory>());
        }

        [Fact]
        public void GetSymmetricEncryptionKeyEncryptedWithSystemKey_ReturnsSymmetricKey()
        {
            var result = _securityService.GetSymmetricEncryptionKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.Key);
            Assert.True(result.Version > -1);
        }

        [Fact]
        public void GetAsymmetricEncryptionKeyEncryptedWithSystemKey_ReturnsAsymmetricKey()
        {
            var result = _securityService.GetAsymmetricEncryptionKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.PublicKey);
            Assert.NotNull(result.PrivateKey);
            Assert.True(result.Version > -1);
        }

        [Fact]
        public void GetAsymmetricSignatureKeyEncryptedWithSystemKey_ReturnsAsymmetricKey()
        {
            var result = _securityService.GetAsymmetricSignatureKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.PublicKey);
            Assert.NotNull(result.PrivateKey);
            Assert.True(result.Version > -1);
        }
    }
}
