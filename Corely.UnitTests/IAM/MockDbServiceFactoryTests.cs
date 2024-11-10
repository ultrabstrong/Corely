using Corely.IAM;
using Corely.Security.KeyStore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.UnitTests.IAM
{
    public class MockDbServiceFactoryTests : ServiceFactoryGenericTests
    {
        private class MockServiceFactory : MockDbServiceFactory
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
        }

        private readonly MockServiceFactory _mockServiceFactory = new();

        protected override ServiceFactoryBase ServiceFactory => _mockServiceFactory;
    }
}
