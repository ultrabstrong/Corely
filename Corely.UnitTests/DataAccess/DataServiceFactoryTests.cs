using AutoFixture;
using Corely.DataAccess;
using Corely.DataAccess.Factories;
using Corely.Domain.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess
{
    public class DataServiceFactoryTests
    {
        private readonly Fixture _fixture = new();
        private readonly ServiceCollection _serviceCollection = new();
        private readonly DataAccessConnection<string> _connection;

        public DataServiceFactoryTests()
        {
            _serviceCollection.AddScoped<ILoggerFactory, LoggerFactory>();
            _connection = new DataAccessConnection<string>(_fixture.Create<string>(), _fixture.Create<string>());
        }

        [Theory, MemberData(nameof(GetRequiredServiceData))]
        public void RegisterConnection_ShouldRegisterConnection(Type serviceType)
        {
            DataServiceFactory.RegisterConnection(_connection, _serviceCollection);
            var serviceProvider = _serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredKeyedService(serviceType, _connection.ConnectionName);

            Assert.NotNull(service);
        }

        public static IEnumerable<object[]> GetRequiredServiceData =>
        [
            [typeof(IDataAccessConnection<string>)],
            [typeof(IGenericRepoFactory<string>)]
        ];
    }
}
