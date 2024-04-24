using Corely.DataAccess.Mock.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos
{
    public class MockRepoExtendedGetTests : RepoExtendedGetTestsBase<EntityFixture>
    {
        private readonly MockRepoExtendedGet<EntityFixture> _mockRepoFixture = new();
        protected override IRepoExtendedGet<EntityFixture> Repo => _mockRepoFixture;
    }
}
