using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.IAM.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class EFRepoExtendedGetTests : RepoExtendedGetTestsBase<EntityFixture>
    {
        private readonly EFRepoExtendedGet<EntityFixture> _efExtendedGetRepo;
        private readonly DbContextFixture _dbContext;

        public EFRepoExtendedGetTests()
        {
            var serviceFactory = new ServiceFactory();
            _dbContext = GetDbContext();

            _efExtendedGetRepo = new EFRepoExtendedGet<EntityFixture>(
                serviceFactory.GetRequiredService<ILogger<EFRepoExtendedGet<EntityFixture>>>(),
                () => _dbContext.SaveChangesAsync(),
                _dbContext.Entities);
        }

        private static DbContextFixture GetDbContext()
        {
            var fixture = new Fixture();
            var options = new DbContextOptionsBuilder<DbContextFixture>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            var dbContext = new DbContextFixture(options);

            return dbContext;
        }

        [Fact]
        public async Task GetAsync_Uses_OrderBy()
        {
            var orderByMock = new Mock<Func<IQueryable<EntityFixture>, IOrderedQueryable<EntityFixture>>>();
            orderByMock
                .Setup(m => m(
                    It.IsAny<IQueryable<EntityFixture>>()))
                .Returns((IQueryable<EntityFixture> q) =>
                    q.OrderBy(u => u.Id));

            await _efExtendedGetRepo.GetAsync(
                u => u.Id == 1,
                orderBy: orderByMock.Object);

            orderByMock.Verify(
                m => m(It.IsAny<IQueryable<EntityFixture>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAsync_Uses_Include()
        {
            var includeMock = new Mock<Func<IQueryable<EntityFixture>, IQueryable<EntityFixture>>>();
            includeMock
                .Setup(m => m(
                    It.IsAny<IQueryable<EntityFixture>>()))
                .Returns((IQueryable<EntityFixture> q) =>
                    q.Include(u => u.NavigationProperty));

            await _efExtendedGetRepo.GetAsync(
                u => u.Id == 1,
                include: includeMock.Object);

            includeMock.Verify(
                m => m(It.IsAny<IQueryable<EntityFixture>>()),
                Times.Once);
        }

        protected override IRepoExtendedGet<EntityFixture> Repo => _efExtendedGetRepo;
    }
}
