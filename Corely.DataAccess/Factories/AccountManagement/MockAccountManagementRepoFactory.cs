using Corely.DataAccess.DataSources.Mock;
using Corely.DataAccess.Repos;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Factories.AccountManagement
{
    internal class MockAccountManagementRepoFactory : IAccountManagementRepoFactory
    {
        // Reuse the same mocks so multiple requests work when testing
        private readonly MockRepoExtendedGet<AccountEntity> _accountRepo = new();
        private readonly MockRepoExtendedGet<UserEntity> _userRepo = new();
        private readonly MockRepoExtendedGet<BasicAuthEntity> _basicAuthRepo = new();
        private readonly MockUoWProvider _mockUoWProvider = new();

        public IRepoExtendedGet<AccountEntity> CreateAccountRepo()
        {
            return _accountRepo;
        }

        public IReadonlyRepo<AccountEntity> CreateReadonlyAccountRepo()
        {
            return new MockReadonlyRepo<AccountEntity>(_accountRepo);
        }

        public IRepoExtendedGet<UserEntity> CreateUserRepo()
        {
            return _userRepo;
        }

        public IRepoExtendedGet<BasicAuthEntity> CreateBasicAuthRepo()
        {
            return _basicAuthRepo;
        }

        public IUnitOfWorkProvider CreateUnitOfWorkProvider()
        {
            return _mockUoWProvider;
        }
    }
}
