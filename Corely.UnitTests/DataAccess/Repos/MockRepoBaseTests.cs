using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class MockRepoBaseTests : RepoTestsBase<EntityFixture>
    {
        private class TestMockRepo : MockRepoBase<EntityFixture> { }

        private readonly TestMockRepo _mockRepoFixture = new();
        protected override IRepo<EntityFixture> Repo => _mockRepoFixture;
    }
}
