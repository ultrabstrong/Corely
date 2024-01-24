using Corely.DataAccess;
using Corely.Domain.Connections;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess
{
    public class DataServiceFactoryTests
    {
        private class TestDataServiceFactory : DataServiceFactory<string>
        {
            private readonly IServiceProvider _serviceProvider;

            public TestDataServiceFactory(IDataAccessConnection<string> connection)
                : base(connection)
            {
                var services = new ServiceCollection();
                services.AddSingleton<ILoggerFactory, LoggerFactory>();
                services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                AddDataServices(services);
                _serviceProvider = services.BuildServiceProvider();
            }

            public T GetRequiredService<T>() where T : notnull
                => _serviceProvider.GetRequiredService<T>();

            public T GetRequiredKeyedService<T>(string key) where T : notnull
                => _serviceProvider.GetRequiredKeyedService<T>(key);
        }

        private readonly TestDataServiceFactory _testDataServiceFactory;

        public DataServiceFactoryTests()
        {
            var connection = new DataAccessConnection<string>(ConnectionNames.Mock, "");
            _testDataServiceFactory = new TestDataServiceFactory(connection);
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
            [typeof(IDataAccessConnection<string>), ConnectionNames.Mock],
            [typeof(IRepoExtendedGet<BasicAuthEntity>)],
            [typeof(IRepoExtendedGet<AccountEntity>)],
            [typeof(IRepoExtendedGet<UserEntity>)]
        ];

        private object? GetRequiredService(Type serviceType)
        {
            var methodInfo = _testDataServiceFactory.GetType()
                .GetMethod(nameof(TestDataServiceFactory.GetRequiredService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(_testDataServiceFactory, null);
        }

        private object? GetRequiredKeyedService(Type serviceType, string key)
        {
            var methodInfo = _testDataServiceFactory.GetType()
                .GetMethod(nameof(TestDataServiceFactory.GetRequiredKeyedService));

            return methodInfo?.MakeGenericMethod(serviceType)
                .Invoke(_testDataServiceFactory, [key]);
        }

    }
}
