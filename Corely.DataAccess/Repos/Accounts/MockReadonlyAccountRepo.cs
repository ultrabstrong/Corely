using Corely.Domain.Entities.Accounts;

namespace Corely.DataAccess.Repos.Accounts
{
    internal sealed class MockReadonlyAccountRepo
        : MockReadonlyRepoBase<AccountEntity>
    {
        private readonly MockAccountRepo _mockAccountRepo;

        public MockReadonlyAccountRepo(MockAccountRepo mockAccountRepo)
        {
            _mockAccountRepo = mockAccountRepo;
        }

        protected override List<AccountEntity> Entities => _mockAccountRepo.GetEntities();
    }
}
