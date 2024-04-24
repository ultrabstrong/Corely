using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.AccountManagement;
using Corely.DataAccess.Factories;
using Corely.DataAccess.Mock;
using Corely.UnitTests.DataAccess.Factories;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.AccountManagement
{
    public class EFAccountManagementRepoFactoryTests : AccountManagementRepoFactoryTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFAccountManagementRepoFactory _factory;

        protected override IAccountManagementRepoFactory AccountManagementRepoFactory => _factory;

        public EFAccountManagementRepoFactoryTests()
        {
            var mockFactory = GetMockFactory();
            mockFactory.Setup(f => f.CreateDbContext())
                .Returns(new AccountManagementDbContext(new EFConfigurationFixture()));

            _factory = mockFactory.Object;
        }

        [Fact]
        public void CreateDbContext_ShouldReturnAccountManagementDbContext()
        {
            var factory = GetMockFactory();
            var accountManagementDbContext = factory.Object.CreateDbContext();
            Assert.NotNull(accountManagementDbContext);
        }

        [Fact]
        public void CreateUnitOfWorkProvider_ShouldReturnMockUow_WithInMemoryDb()
        {
            var factory = GetMockFactory();
            var uowProvider = factory.Object.CreateUnitOfWorkProvider();
            Assert.NotNull(uowProvider);
            Assert.IsType<MockUoWProvider>(uowProvider);
        }

        private Mock<EFAccountManagementRepoFactory> GetMockFactory()
        {
            var mockFactory = new Mock<EFAccountManagementRepoFactory>(
                _serviceFactory.GetRequiredService<ILoggerFactory>(),
                new Mock<EFConnection>(new EFConfigurationFixture()).Object)
            {
                CallBase = true
            };

            return mockFactory;
        }
    }
}
