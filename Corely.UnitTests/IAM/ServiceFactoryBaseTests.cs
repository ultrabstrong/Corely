using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.IAM;
using Corely.IAM.AccountManagement.Services;
using Corely.IAM.Accounts.Services;
using Corely.IAM.Auth.Services;
using Corely.IAM.Mappers;
using Corely.IAM.Users.Services;
using Corely.IAM.Validators;
using Corely.Security.Encryption.Factories;
using Corely.Security.Hashing.Factories;
using Corely.Security.KeyStore;
using Corely.Security.PasswordValidation.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM
{
    public class ServiceFactoryBaseTests
    {
        private class MockServiceFactory : ServiceFactoryBase
        {
            protected override void AddLogger(IServiceCollection services)
            {
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            }

            protected override void AddDataAccessServices(IServiceCollection services)
            {
                var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");
                var keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, services);
                keyedDataServiceFactory.AddAllDataServices(services);
            }
        }

        private readonly MockServiceFactory _mockServiceFactory = new();

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
            [typeof(IPasswordValidationProvider)],

            [typeof(IMapProvider)],

            [typeof(IValidationProvider)],

            [typeof(IKeyStoreProvider)],
            [typeof(IEncryptionProviderFactory)],
            [typeof(IHashProviderFactory)],

            [typeof(IAuthService)],
            [typeof(IAccountService)],
            [typeof(IUserService)],
            [typeof(IAccountManagementService)]
        ];

        private object? GetRequiredService(Type serviceType)
        {
            var methodInfo = _mockServiceFactory.GetType()
                .GetMethod(nameof(MockServiceFactory.GetRequiredService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(_mockServiceFactory, null);
        }

        private object? GetRequiredKeyedService(Type serviceType, string key)
        {
            var methodInfo = _mockServiceFactory.GetType()
                .GetMethod(nameof(MockServiceFactory.GetRequiredKeyedService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(_mockServiceFactory, [key]);
        }

        [Fact]
        public void ServiceFactoryBase_ShouldDisposeServiceProviderCorrectly()
        {
            var mockServiceFactory = new MockServiceFactory();

            mockServiceFactory.Dispose();
            var ex = Record.Exception(() => mockServiceFactory.GetRequiredService<ILogger>());

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }
    }
}
