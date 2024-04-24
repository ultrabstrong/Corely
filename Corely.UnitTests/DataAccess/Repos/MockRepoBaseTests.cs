using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class MockRepoBaseTests : RepoTestsBase<EntityFixture>
    {
        private readonly MockRepo<EntityFixture> _mockRepoFixture = new();
        protected override IRepo<EntityFixture> Repo => _mockRepoFixture;
    }
}
