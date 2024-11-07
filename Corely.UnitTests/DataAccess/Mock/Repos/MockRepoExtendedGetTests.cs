using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Mock.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos
{
    public class MockRepoExtendedGetTests : RepoExtendedGetTestsBase
    {
        private readonly MockRepoExtendedGet<EntityFixture> _mockRepoFixture = new(
            new MockRepo<EntityFixture>());
        protected override IRepoExtendedGet<EntityFixture> Repo => _mockRepoFixture;
    }
}
