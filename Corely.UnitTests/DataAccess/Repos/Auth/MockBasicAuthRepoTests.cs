using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    public class MockBasicAuthRepoTests : BasicAuthRepoTestsBase
    {
        private readonly MockBasicAuthRepo _mockBasicAuthRepo = new();
        protected override IRepoExtendedGet<BasicAuthEntity> Repo => _mockBasicAuthRepo;
    }
}
