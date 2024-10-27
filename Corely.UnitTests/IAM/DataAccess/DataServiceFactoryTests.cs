using Corely.DataAccess.Connections;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.DataAccess;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.Users.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.DataAccess
{
    public class DataServiceFactoryTests
    {
        private const string CONNECTION_NAME = ConnectionNames.Mock;

        private readonly ServiceProvider _serviceProvider;

        public DataServiceFactoryTests()
        {
            var connection = new DataAccessConnection<string>(CONNECTION_NAME, string.Empty);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            DataServiceFactory.RegisterConnection(connection, serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

        }

        [Theory, MemberData(nameof(GetRequiredKeyedServiceData))]
        public void RegisterConnection_RegistersConnection(Type serviceType)
        {
            var service = _serviceProvider.GetRequiredKeyedService(serviceType, CONNECTION_NAME);
            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredKeyedServiceData =>
        [
            [typeof(IDataAccessConnection<string>)],
            [typeof(IGenericRepoFactory)]
        ];

        [Theory, MemberData(nameof(GetRequiredServiceData))]
        public void RegisterConnection_RegistersService(Type serviceType)
        {
            var service = _serviceProvider.GetRequiredService(serviceType);
            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(IIAMRepoFactory)],
            [typeof(IRepoExtendedGet<AccountEntity>)],
            [typeof(IReadonlyRepo<AccountEntity>)],
            [typeof(IRepoExtendedGet<UserEntity>)],
            [typeof(IReadonlyRepo<UserEntity>)],
            [typeof(IRepoExtendedGet<BasicAuthEntity>)],
            [typeof(IUnitOfWorkProvider)]
        ];
    }
}
