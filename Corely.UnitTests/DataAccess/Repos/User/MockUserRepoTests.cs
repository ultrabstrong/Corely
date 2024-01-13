using Corely.DataAccess.Repos.User;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public class MockUserRepoTests : UserRepoTestsBase
    {
        private readonly MockUserRepo _mockUserRepo = new();
        protected override IUserRepo MockUserRepo => _mockUserRepo;
    }
}
