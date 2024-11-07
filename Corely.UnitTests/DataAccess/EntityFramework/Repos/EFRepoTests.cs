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
        private readonly DbSet<EntityFixture> _dbSet;

        private readonly EntityFixture _setupEntity = new() { Id = 1 };
        private readonly EntityFixture _testEntity = new() { Id = 2 };

        public EFRepoTests()
        {
            var serviceFactory = new ServiceFactory();

            var dbContext = new DbContextFixture(
                new DbContextOptionsBuilder<DbContextFixture>()
                    .UseInMemoryDatabase(databaseName: new Fixture().Create<string>())
                    .Options);

            _dbSet = dbContext.Set<EntityFixture>();
            _dbSet.Add(_setupEntity);
            dbContext.SaveChanges();

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
            var entity = await _efRepo.GetAsync(_setupEntity.Id);
            Assert.Equal(_setupEntity, entity);
        }

        [Fact]
        public async Task UpdateAsync_AttachesUntrackedEntity()
        {
            _dbSet.Entry(_setupEntity).State = EntityState.Detached;

            var entity = new EntityFixture { Id = _setupEntity.Id };
            await _efRepo.UpdateAsync(entity);

            var updatedEntity = _dbSet.Find(entity.Id);
            Assert.NotNull(updatedEntity);

            Assert.NotEqual(_setupEntity, updatedEntity);
            Assert.Equal(entity, updatedEntity);

            Assert.InRange(
                updatedEntity.ModifiedUtc,
                DateTime.UtcNow.AddSeconds(-2),
                DateTime.UtcNow);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingEntity()
        {
            var untrackedSetupEntity = new EntityFixture { Id = _setupEntity.Id };

            await _efRepo.UpdateAsync(untrackedSetupEntity);

            // UpdateAsync automatically updates the ModifiedUtc
            // It should find and update ModifiedUtc of the original entity
            // This has the added benefit of testing the ModifiedUtc update
            Assert.InRange(
                _setupEntity.ModifiedUtc,
                DateTime.UtcNow.AddSeconds(-2),
                DateTime.UtcNow);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntity()
        {
            await _efRepo.DeleteAsync(_setupEntity);

            var entity = _dbSet.Find(_setupEntity.Id);

            Assert.Null(entity);
        }
    }
}
