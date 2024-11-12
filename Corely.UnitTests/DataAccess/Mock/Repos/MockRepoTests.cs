using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Mock.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos
{
    public class MockRepoTests : RepoTestsBase
    {
        private readonly MockRepo<EntityFixture> _mockRepoFixture = new();
        protected override IRepo<EntityFixture> Repo => _mockRepoFixture;
    }
}
