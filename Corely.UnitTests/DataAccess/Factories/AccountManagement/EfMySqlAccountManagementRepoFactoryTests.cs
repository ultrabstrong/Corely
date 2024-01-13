using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Factories.AccountManagement;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Corely.UnitTests.DataAccess.Factories.AccountManagement
{
    [Collection(CollectionNames.ServiceFactory)]
    public class EfMySqlAccountManagementRepoFactoryTests : AccountManagementRepoFactoryTestsBase
    {
        private const string MYSQL_CONNECTION_STRING = "Server=localhost;Database=default;Uid=user;Pwd=password;";

        private readonly EfMySqlAccountManagementRepoFactory _factory;
        private readonly ServiceFactory _serviceFactory;

        protected override IAccountManagementRepoFactory AccountManagementRepoFactory => _factory;

        public EfMySqlAccountManagementRepoFactoryTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;

            var options = new DbContextOptionsBuilder<AccountManagementDbContext>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            var mockFactory = GetMockFactory();
            mockFactory.Setup(f => f.CreateDbContext())
                .Returns(new AccountManagementDbContext(options));

            _factory = mockFactory.Object;
        }

        private Mock<EfMySqlAccountManagementRepoFactory> GetMockFactory()
        {
            var mockFactory = new Mock<EfMySqlAccountManagementRepoFactory>(
                _serviceFactory.GetRequiredService<ILoggerFactory>(),
                MYSQL_CONNECTION_STRING)
            {
                CallBase = true
            };

            return mockFactory;
        }

        [Fact]
        public void CreateDbContext_ShouldReturnAccountManagementDbContext()
        {
            var factory = GetMockFactory();
            factory.Setup(f => f.GetServerVersion())
                .Returns(ServerVersion.Create(new Version(), ServerType.MySql));

            var accountManagementDbContext = factory.Object.CreateDbContext();
            Assert.NotNull(accountManagementDbContext);
        }
    }
}
