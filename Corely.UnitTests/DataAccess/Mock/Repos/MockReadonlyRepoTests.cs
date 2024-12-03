using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Mock.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos;

public class MockReadonlyRepoTests : ReadonlyRepoTestsBase
{
    private readonly MockRepo<EntityFixture> _mockRepo = new();
    private readonly MockReadonlyRepo<EntityFixture> _mockReadonlyRepo;

    public MockReadonlyRepoTests()
    {
        _mockReadonlyRepo = new MockReadonlyRepo<EntityFixture>(_mockRepo);
    }

    protected override IReadonlyRepo<EntityFixture> ReadonlyRepo => _mockReadonlyRepo;

    protected override int FillRepoAndReturnId()
    {
        var entityList = Fixture.CreateMany<EntityFixture>(5).ToList();
        foreach (var entity in entityList)
        {
            _mockRepo.CreateAsync(entity);
        }

        return entityList[2].Id;
    }
}
