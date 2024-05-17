using Corely.IAM;
using Corely.IAM.Security.Services;
using Corely.Security.Encryption.Factories;
using Corely.Security.Keys.Symmetric;

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
                serviceFactory.GetRequiredService<ISymmetricKeyProvider>(),
                serviceFactory.GetRequiredService<ISymmetricEncryptionProviderFactory>());
        }

        [Fact]
        public void GetSymmetricKeyEncryptedWithSystemKey_ReturnsSymmetricKey()
        {
            var result = _securityService.GetSymmetricKeyEncryptedWithSystemKey();

            Assert.NotNull(result);
            Assert.NotNull(result.Key);
            Assert.True(result.Version > -1);
        }
    }
}
