using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.IAM;
using Corely.DataAccess.Mock;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.IAM
{
    public class EFIAMRepoFactoryTests : IAMRepoFactoryTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFIAMRepoFactory _factory;

        protected override IIAMRepoFactory AccountManagementRepoFactory => _factory;

        public EFIAMRepoFactoryTests()
        {
            var mockFactory = GetMockFactory();
            mockFactory.Setup(f => f.CreateDbContext())
                .Returns(new IAMDbContext(new EFConfigurationFixture()));

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

        private Mock<EFIAMRepoFactory> GetMockFactory()
        {
            var mockFactory = new Mock<EFIAMRepoFactory>(
                _serviceFactory.GetRequiredService<ILoggerFactory>(),
                new Mock<EFConnection>(new EFConfigurationFixture()).Object)
            {
                CallBase = true
            };

            return mockFactory;
        }
    }
}
