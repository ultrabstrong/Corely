using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.UnitTests.Fixtures;
using FluentAssertions;

namespace Corely.UnitTests.DataAccess;

public abstract class RepoTestsBase : ReadonlyRepoTestsBase
{
    protected override IReadonlyRepo<EntityFixture> ReadonlyRepo => Repo;

    protected abstract IRepo<EntityFixture> Repo { get; }

    [Fact]
    public async Task Create_ThenGet_ReturnsAdded()
    {
        var entity = Fixture.Create<EntityFixture>();

        await Repo.CreateAsync(entity);
        var result = await Repo.GetAsync(entity.Id);

        Assert.Equal(entity, result);
    }

    [Fact]
    public async Task Create_ThenList_ReturnsAllAdded()
    {
        var entities = Fixture.CreateMany<EntityFixture>().ToArray();
        await Repo.CreateAsync(entities);
        var result = await Repo.ListAsync();
        result.Should().BeEquivalentTo(entities);
    }

    [Fact]
    public async Task Create_ThenUpdate_Updates()
    {
        var entity = Fixture.Create<EntityFixture>();
        await Repo.CreateAsync(entity);

        var updateEntity = Fixture.Create<EntityFixture>();
        updateEntity.Id = entity.Id;
        updateEntity.CreatedUtc = entity.CreatedUtc;
        await Repo.UpdateAsync(updateEntity);

        var result = await Repo.GetAsync(entity.Id);

        result.Should()
            .BeEquivalentTo(entity, options => options
            .Excluding(m => m.ModifiedUtc)
            .Excluding(m => m.NavigationProperty));
    }

    [Fact]
    public async Task Create_ThenUpdate_UpdatesModifiedUtc()
    {
        var entity = Fixture.Create<EntityFixture>();
        entity.ModifiedUtc = DateTime.UtcNow;
        var originalModifiedUtc = entity.ModifiedUtc;

        var updateEntity = Fixture.Create<EntityFixture>();
        updateEntity.Id = entity.Id;

        await Repo.CreateAsync(entity);
        await Repo.UpdateAsync(updateEntity);
        var result = await Repo.GetAsync(entity.Id);

        Assert.NotNull(result);
        Assert.True(originalModifiedUtc < updateEntity.ModifiedUtc);
    }

    [Fact]
    public async Task Create_ThenDelete_Deletes()
    {
        var entity = Fixture.Create<EntityFixture>();

        await Repo.CreateAsync(entity);
        await Repo.DeleteAsync(entity);
        var result = await Repo.GetAsync(entity.Id);

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ThenDeleteById_Deletes()
    {
        var entity = Fixture.Create<EntityFixture>();

        await Repo.CreateAsync(entity);
        await Repo.DeleteAsync(entity.Id);
        var result = await Repo.GetAsync(entity.Id);

        Assert.Null(result);
    }
}
