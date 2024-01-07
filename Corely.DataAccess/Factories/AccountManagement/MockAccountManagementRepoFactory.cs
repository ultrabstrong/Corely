using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class MockAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        // Reuse the same mocks so multiple requests work when testing
        private readonly MockBasicAuthRepo _basicAuthRepo = new();
        private readonly MockUserRepo _userRepo = new();

        public IAuthRepo<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return _basicAuthRepo;
        }

        public IUserRepo CreateUserRepo()
        {
            return _userRepo;
        }
    }
}
