using Corely.IAM;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.UnitTests
{
    public class ServiceFactory : MockDbServiceFactory
    {
        protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        {
            return new SecurityConfigurationProvider();
        }

        protected override void AddLogging(ILoggingBuilder builder)
        {
            builder.AddProvider(NullLoggerProvider.Instance);
        }
    }
}
