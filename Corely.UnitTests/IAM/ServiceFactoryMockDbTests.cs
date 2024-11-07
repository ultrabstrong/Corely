using Corely.IAM;
using Corely.Security.KeyStore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.UnitTests.IAM
{
    public class ServiceFactoryMockDbTests : ServiceFactoryGenericTests
    {
        private class MockServiceFactory : ServiceFactoryMockDb
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
