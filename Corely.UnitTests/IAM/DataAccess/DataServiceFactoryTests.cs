using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Interfaces.UnitOfWork;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.DataAccess;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.Mock;
using Corely.IAM.Users.Entities;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.DataAccess
{
    public class DataServiceFactoryTests
    {
        private const string CONNECTION_NAME = ConnectionNames.Mock;

        private static readonly Fixture _fixture = new();
        private readonly ServiceProvider _serviceProvider;

        public DataServiceFactoryTests()
        {
            var connection = new DataAccessConnection<string>(CONNECTION_NAME, string.Empty);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            DataServiceFactory.RegisterConnection(connection, serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

        }

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

        [Fact]
        public void CreateIAMRepoFactory_ReturnsCorrectType_WithMockConnection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            var connection = new DataAccessConnection<string>(
                ConnectionNames.Mock, _fixture.Create<string>());

            DataServiceFactory.RegisterConnection(connection, serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var repoFactory = serviceProvider.GetRequiredService<IIAMRepoFactory>();

            Assert.NotNull(repoFactory);
            Assert.IsType<MockIAMRepoFactory>(repoFactory);
        }

        [Fact]
        public void CreateIAMRepoFactory_ReturnsCorrectType_WithEFConnection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            var configuration = new EFConfigurationFixture();
            var connection = new DataAccessConnection<EFConnection>(
                ConnectionNames.EntityFramework, new EFConnection(configuration));

            DataServiceFactory.RegisterConnection(connection, serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var repoFactory = serviceProvider.GetRequiredService<IIAMRepoFactory>();

            Assert.NotNull(repoFactory);
            Assert.IsType<EFIAMRepoFactory>(repoFactory);
        }

        [Fact]
        public void CreateIAMRepoFactory_ThrowsArgumentOutOfRangeException_WithInvalidConnectionName()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            var connection = new DataAccessConnection<string>(
                _fixture.Create<string>(), _fixture.Create<string>());

            DataServiceFactory.RegisterConnection(connection, serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var ex = Record.Exception(() => serviceProvider.GetRequiredService<IIAMRepoFactory>());

            Assert.NotNull(ex);
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }
    }
}
