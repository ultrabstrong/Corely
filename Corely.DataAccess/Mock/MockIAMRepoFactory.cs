using Corely.DataAccess.EntityFramework.IAM;
using Corely.DataAccess.Mock.Repos;
using Corely.IAM.Accounts.Entities;
using Corely.IAM.Auth.Entities;
using Corely.IAM.Repos;
using Corely.IAM.Users.Entities;

namespace Corely.DataAccess.Mock
{
    internal class MockIAMRepoFactory : IIAMRepoFactory
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
