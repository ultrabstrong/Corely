using AutoFixture;
using Corely.DataAccess;
using Corely.DataAccess.Connections;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess
{
    public class KeyedDataServiceFactoryTests
    {
        private readonly Fixture _fixture = new();
        private readonly ServiceCollection _serviceCollection = new();
        private readonly KeyedDataServiceFactory<string> _keyedDataServiceFactory;

        public KeyedDataServiceFactoryTests()
        {
            _serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
            var connection = new DataAccessConnection<string>(ConnectionNames.Mock, _fixture.Create<string>());
            _keyedDataServiceFactory = DataServiceFactory.RegisterConnection(connection, _serviceCollection);
        }

        [Theory, MemberData(nameof(GetRequiredServiceData))]
        public void AddAccountManagementDataAccessServices_ShouldAddAccountManagementDataAccessServices(Type serviceType)
        {
            _keyedDataServiceFactory.AddAccountManagementDataAccessServices(_serviceCollection);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService(serviceType);

            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(IAccountManagementRepoFactory)],
            [typeof(IRepoExtendedGet<BasicAuthEntity>)],
            [typeof(IRepoExtendedGet<AccountEntity>)],
            [typeof(IRepoExtendedGet<UserEntity>)],

        ];
    }
}
