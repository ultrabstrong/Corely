using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class EFRepoTests : RepoTestsBase<EntityFixture>
    {
        protected override IRepo<EntityFixture> Repo => _efRepo;
        private readonly EFRepo<EntityFixture> _efRepo;

        private readonly Mock<Func<Task>> _mockSaveChangesAsync = new();
        private readonly Mock<DbSet<EntityFixture>> _mockDbSet;
        private readonly EFRepo<EntityFixture> _mockEfRepo;

        private readonly EntityFixture _entity = new() { Id = 1 };

        public EFRepoTests()
        {
            var serviceFactory = new ServiceFactory();

            var dbContext = new DbContextFixture(
                new DbContextOptionsBuilder<DbContextFixture>()
                    .UseInMemoryDatabase(databaseName: new Fixture().Create<string>())
                    .Options);

            var logger = serviceFactory.GetRequiredService<ILogger<EFRepo<EntityFixture>>>();

            _efRepo = new EFRepo<EntityFixture>(
                logger,
                () => dbContext.SaveChangesAsync(),
                dbContext.Entities);

            _mockDbSet = GetMockDbSet(dbContext);

            _mockEfRepo = new EFRepo<EntityFixture>(
                logger,
                _mockSaveChangesAsync.Object,
                _mockDbSet.Object);
        }

        private static Mock<DbSet<EntityFixture>> GetMockDbSet(DbContextFixture dbContext)
        {
            var mockDbSet = new Mock<DbSet<EntityFixture>>();

            mockDbSet
                .Setup(m => m.Local)
                .Returns(dbContext.Entities.Local);

            mockDbSet
                .Setup(m => m.AddAsync(
                    It.IsAny<EntityFixture>(),
                    It.IsAny<CancellationToken>()))
                .Returns((EntityFixture entity, CancellationToken cancellationToken) =>
                    dbContext.AddAsync(entity, cancellationToken));

            mockDbSet
                .Setup(m => m.Attach(It.IsAny<EntityFixture>()))
                .Returns<EntityFixture>(dbContext.Entities.Attach);

            mockDbSet
                .Setup(m => m.Entry(It.IsAny<EntityFixture>()))
                .Returns<EntityFixture>(dbContext.Entities.Entry);

            return mockDbSet;
        }

        [Fact]
        public async Task CreateAsync_AddsEntity()
        {
            await _mockEfRepo.CreateAsync(_entity);

            _mockDbSet
                .Verify(m => m.AddAsync(
                    _entity,
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mockSaveChangesAsync.Verify(m => m(), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ReturnsEntity()
        {
            await _mockEfRepo.GetAsync(_entity.Id);

            _mockDbSet.Verify(m => m.FindAsync(_entity.Id), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_AttachesUntrackedEntity()
        {
            await _mockEfRepo.UpdateAsync(_entity);

            _mockDbSet.Verify(m => m.Attach(_entity), Times.Once);
            _mockSaveChangesAsync.Verify(m => m(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingEntity()
        {
            _mockDbSet.Object.Local.Add(_entity);

            await _mockEfRepo.UpdateAsync(_entity);

            _mockDbSet.Verify(m => m.Entry(_entity), Times.Once);
            _mockSaveChangesAsync.Verify(m => m(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntity()
        {
            await _mockEfRepo.DeleteAsync(_entity);

            _mockDbSet.Verify(m => m.Remove(_entity), Times.Once);
            _mockSaveChangesAsync.Verify(m => m(), Times.Once);
        }
    }
}
