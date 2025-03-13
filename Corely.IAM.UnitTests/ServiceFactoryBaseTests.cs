using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.DataAccess.Mock;
using Corely.DataAccess.Mock.Repos;
using Corely.IAM;
using Corely.Security.KeyStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Corely.IAM.UnitTests;

public class ServiceFactoryBaseTests : ServiceFactoryGenericTests
{
    private class MockServiceFactory : ServiceFactoryBase
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

        protected override void AddDataServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IReadonlyRepo<>), typeof(MockReadonlyRepo<>));
            services.AddSingleton(typeof(IRepo<>), typeof(MockRepo<>));
            services.AddSingleton<IUnitOfWorkProvider, MockUoWProvider>();
        }
    }

    private readonly MockServiceFactory _mockServiceFactory = new();

    protected override ServiceFactoryBase ServiceFactory => _mockServiceFactory;
}
