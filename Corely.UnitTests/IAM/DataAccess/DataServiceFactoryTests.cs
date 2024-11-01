using AutoFixture;
using Corely.DataAccess.Connections;
using Corely.DataAccess.EntityFramework;
using Corely.IAM.DataAccess;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.IAM.DataAccess.Mock;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.DataAccess
{
    public class DataServiceFactoryTests
    {
        private const string CONNECTION_NAME = ConnectionNames.Mock;

        private static readonly Fixture _fixture = new();

        [Fact]
        public void RegisterConnection_RegistersIAMRepoFactory()
        {
            var connection = new DataAccessConnection<string>(CONNECTION_NAME, string.Empty);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();

            DataServiceFactory.RegisterConnection(connection, serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IIAMRepoFactory>();
            Assert.NotNull(service);
        }

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
