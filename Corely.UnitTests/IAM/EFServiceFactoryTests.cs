using Corely.DataAccess.EntityFramework.Configurations;
using Corely.IAM;
using Corely.Security.KeyStore;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.UnitTests.IAM;

public class EFServiceFactoryTests : ServiceFactoryGenericTests
{
    private class MockServiceFactory : EFServiceFactory
    {
        private class MockSecurityConfiguraitonProvider : ISecurityConfigurationProvider
        {
            public ISymmetricKeyStoreProvider GetSystemSymmetricKey() => null!;
        }

        protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        {
            return new MockSecurityConfiguraitonProvider();
        }

        protected override void AddLogging(ILoggingBuilder builder)
        {
            builder.AddProvider(NullLoggerProvider.Instance);
        }

        protected override IEFConfiguration GetEFConfiguraiton()
        {
            return new EFConfigurationFixture();
        }
    }

    private readonly MockServiceFactory _mockServiceFactory = new();

    protected override ServiceFactoryBase ServiceFactory => _mockServiceFactory;
}
