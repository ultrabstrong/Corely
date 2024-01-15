using Corely.DataAccess.Repos.Accounts;
using Corely.DataAccess.Repos.Auth;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class MockAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        // Reuse the same mocks so multiple requests work when testing
        private readonly MockAccountRepo _accountRepo = new();
        private readonly MockUserRepo _userRepo = new();
        private readonly MockBasicAuthRepo _basicAuthRepo = new();

        public IRepoExtendedGet<AccountEntity> CreateAccountRepo()
        {
            return _accountRepo;
        }

        public IRepoExtendedGet<UserEntity> CreateUserRepo()
        {
            return _userRepo;
        }

        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return _basicAuthRepo;
        }
    }
}
