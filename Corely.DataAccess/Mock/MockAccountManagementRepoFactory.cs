using Corely.DataAccess.Factories;
using Corely.DataAccess.Mock.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Entities.Users;
using Corely.IAM.Repos;

namespace Corely.DataAccess.Mock
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
