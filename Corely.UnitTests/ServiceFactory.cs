using Corely.IAM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.UnitTests
{
    public class ServiceFactory : MockDbServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceFactory()
        {
            var serviceCollection = new ServiceCollection();
            AddIAMServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }


        protected override ISecurityConfigurationProvider GetSecurityConfigurationProvider()
        {
            return new SecurityConfigurationProvider();
        }

        protected override void AddLogging(ILoggingBuilder builder)
        {
            builder.AddProvider(NullLoggerProvider.Instance);
        }

        public T GetRequiredService<T>() where T : notnull
            => _serviceProvider.GetRequiredService<T>();
    }
}
