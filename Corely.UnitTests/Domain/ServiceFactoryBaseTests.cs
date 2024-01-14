using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.Domain.Connections;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Mappers;
using Corely.Domain.Repos;
using Corely.Domain.Services.Users;
using Corely.Domain.Validators;
using Corely.UnitTests.Collections;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Domain
{
    [Collection(CollectionNames.ServiceFactory)]
    public class ServiceFactoryBaseTests
    {
        private readonly ServiceFactory _serviceFactory;

        public ServiceFactoryBaseTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        [Theory, MemberData(nameof(GetRequiredServiceData))]
        public void ServiceFactoryBase_ShouldProvideService(Type serviceType, string key = null)
        {
            var service = key == null
                ? GetRequiredService(serviceType)
                : GetRequiredKeyedService(serviceType, key);

            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(ILoggerFactory)],
            [typeof(ILogger<ServiceFactoryBaseTests>)],

            [typeof(IMapProvider)],

            [typeof(IValidationProvider)],

            [typeof(IKeyStoreProvider)],
            [typeof(IEncryptionProviderFactory)],
            [typeof(IHashProviderFactory)],

            [typeof(IDataAccessConnection<string>), ConnectionNames.Mock],
            [typeof(IAuthRepo<BasicAuthEntity>)],
            [typeof(IUserRepo)],

            [typeof(IUserService)]
        ];

        private object? GetRequiredService(Type serviceType)
        {
            var methodInfo = _serviceFactory.GetType()
                .GetMethod(nameof(ServiceFactory.GetRequiredService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(_serviceFactory, null);
        }

        private object? GetRequiredKeyedService(Type serviceType, string key)
        {
            var methodInfo = _serviceFactory.GetType()
                .GetMethod(nameof(ServiceFactory.GetRequiredKeyedService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(_serviceFactory, [key]);
        }

        [Fact]
        public void ServiceFactoryBase_ShouldDisposeServiceProviderCorrectly()
        {
            var serviceFactory = new ServiceFactory();

            serviceFactory.Dispose();
            var ex = Record.Exception(() => serviceFactory.GetRequiredService<ILogger>());

            Assert.True(ex is ObjectDisposedException);
        }
    }
}
