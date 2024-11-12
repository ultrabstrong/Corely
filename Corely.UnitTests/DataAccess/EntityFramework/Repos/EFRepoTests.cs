using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class EFRepoTests : RepoTestsBase
    {
        protected override IRepo<EntityFixture> Repo => _efRepo;

        private readonly EFRepo<EntityFixture> _efRepo;
        private readonly DbSet<EntityFixture> _dbSet;

        private readonly EntityFixture _testEntity = new() { Id = 1 };

        public EFRepoTests()
        {
            var serviceFactory = new ServiceFactory();

            var dbContext = new DbContextFixture(
                new DbContextOptionsBuilder<DbContextFixture>()
                    .UseInMemoryDatabase(databaseName: new Fixture().Create<string>())
                    .Options);

            _dbSet = dbContext.Set<EntityFixture>();

            var logger = serviceFactory.GetRequiredService<ILogger<EFRepo<EntityFixture>>>();

            _efRepo = new EFRepo<EntityFixture>(
                logger,
                dbContext);
        }

        [Fact]
        public async Task CreateAsync_AddsEntity()
        {
            await _efRepo.CreateAsync(_testEntity);

            var entity = _dbSet.Find(_testEntity.Id);

            Assert.Equal(_testEntity, entity);
        }

        [Fact]
        public async Task GetAsync_ReturnsEntity()
        {
            await _efRepo.CreateAsync(_testEntity);

            var entity = await _efRepo.GetAsync(_testEntity.Id);
            Assert.Equal(_testEntity, entity);
        }

        [Fact]
        public async Task UpdateAsync_AttachesUntrackedEntity()
        {
            await _efRepo.CreateAsync(_testEntity);
            _dbSet.Entry(_testEntity).State = EntityState.Detached;

            var entity = new EntityFixture { Id = _testEntity.Id };
            await _efRepo.UpdateAsync(entity);

            var updatedEntity = _dbSet.Find(entity.Id);
            Assert.NotNull(updatedEntity);

            Assert.NotEqual(_testEntity, updatedEntity);
            Assert.Equal(entity, updatedEntity);

            Assert.InRange(
                updatedEntity.ModifiedUtc,
                DateTime.UtcNow.AddSeconds(-2),
                DateTime.UtcNow);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingEntity()
        {
            await _efRepo.CreateAsync(_testEntity);
            var untrackedSetupEntity = new EntityFixture { Id = _testEntity.Id };

            await _efRepo.UpdateAsync(untrackedSetupEntity);

            // UpdateAsync automatically updates the ModifiedUtc
            // It should find and update ModifiedUtc of the original entity
            // This has the added benefit of testing the ModifiedUtc update
            Assert.InRange(
                _testEntity.ModifiedUtc,
                DateTime.UtcNow.AddSeconds(-2),
                DateTime.UtcNow);
        }

        [Fact]
        public async Task DeleteAsync_HandlesNonexistantEntity()
        {
            var ex = await Record.ExceptionAsync(() => _efRepo.DeleteAsync(_testEntity));

            Assert.Null(ex);
        }

        [Fact]
        public async Task DeleteAsync_Id_HandlesNonexistantEntity()
        {
            var ex = await Record.ExceptionAsync(() => _efRepo.DeleteAsync(_testEntity.Id));

            Assert.Null(ex);
        }
    }
}
