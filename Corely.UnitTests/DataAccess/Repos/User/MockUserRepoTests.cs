using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public class MockUserRepoTests : UserRepoTestsBase
    {
        private readonly MockUserRepo _mockUserRepo = new();
        protected override IRepoExtendedGet<UserEntity> Repo => _mockUserRepo;
    }
}
