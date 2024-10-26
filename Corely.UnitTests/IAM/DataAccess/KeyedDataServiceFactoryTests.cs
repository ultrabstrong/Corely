using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.Interfaces.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.DataAccess;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.Users.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.DataAccess
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
        public void AddIAMDataAccessServices_AddsIAMDataAccessServices(Type serviceType)
        {
            _keyedDataServiceFactory.AddIAMDataAccessServices(_serviceCollection);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService(serviceType);

            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(IIAMRepoFactory)],
            [typeof(IRepoExtendedGet<AccountEntity>)],
            [typeof(IReadonlyRepo<AccountEntity>)],
            [typeof(IRepoExtendedGet<UserEntity>)],
            [typeof(IRepoExtendedGet<BasicAuthEntity>)],
        ];
    }
}
