using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.DataAccess.UnitTests.EntityFramework.Repos;

public class EFRepoTests : RepoTestsBase
{
    private readonly EFRepo<EntityFixture> _efRepo;
    private readonly DbContext _dbContext;

    private readonly EntityFixture _testEntity = new() { Id = 1 };

    public EFRepoTests()
    {
        _dbContext = new DbContextFixture(
            new DbContextOptionsBuilder<DbContextFixture>()
                .UseInMemoryDatabase(databaseName: new Fixture().Create<string>())
                .Options);

        _efRepo = new EFRepo<EntityFixture>(
            Moq.Mock.Of<ILogger<EFRepo<EntityFixture>>>(),
            _dbContext);
    }

    protected override IRepo<EntityFixture> Repo => _efRepo;

    [Fact]
    public async Task CreateAsync_AddsEntity()
    {
        await _efRepo.CreateAsync(_testEntity);

        var entity = _dbContext.Set<EntityFixture>().Find(_testEntity.Id);

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
        _dbContext.Set<EntityFixture>().Entry(_testEntity).State = EntityState.Detached;

        var entity = new EntityFixture { Id = _testEntity.Id };
        await _efRepo.UpdateAsync(entity);

        var updatedEntity = _dbContext.Set<EntityFixture>().Find(entity.Id);
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

    protected override int FillRepoAndReturnId()
    {
        _dbContext.Set<EntityFixture>().AddRange(Fixture.CreateMany<EntityFixture>(5));
        _dbContext.SaveChanges();
        return _dbContext.Set<EntityFixture>().Skip(1).First().Id;
    }
}
