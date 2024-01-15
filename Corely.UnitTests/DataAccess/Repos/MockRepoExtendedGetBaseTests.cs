using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class MockRepoExtendedGetBaseTests : RepoExtendedGetTestsBase<EntityFixture>
    {
        private class TestMockRepo : MockRepoExtendedGetBase<EntityFixture> { }

        private readonly TestMockRepo _mockRepoFixture = new();
        protected override IRepoExtendedGet<EntityFixture> Repo => _mockRepoFixture;
    }
}
