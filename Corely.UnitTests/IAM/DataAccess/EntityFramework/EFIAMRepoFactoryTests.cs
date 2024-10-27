using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.Mock;
using Corely.IAM.DataAccess.EntityFramework;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.IAM.DataAccess.EntityFramework
{
    public class EFIAMRepoFactoryTests : IAMRepoFactoryTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFIAMRepoFactory _factory;

        protected override IIAMRepoFactory IAMRepoFactory => _factory;

        public EFIAMRepoFactoryTests()
        {
            var mockFactory = GetMockFactory();
            mockFactory.Setup(f => f.CreateDbContext())
                .Returns(new IAMDbContext(new EFConfigurationFixture()));

            _factory = mockFactory.Object;
        }

        [Fact]
        public void CreateDbContext_ReturnsIAMDbContext()
        {
            var factory = GetMockFactory();
            var iamDbContext = factory.Object.CreateDbContext();
            Assert.NotNull(iamDbContext);
        }

        [Fact]
        public void CreateUnitOfWorkProvider_ReturnsMockUow_WithInMemoryDb()
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
