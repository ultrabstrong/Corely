using Corely.Domain.Entities.Accounts;

namespace Corely.DataAccess.Repos.Accounts
{
    internal sealed class MockReadonlyAccountRepo
        : MockReadonlyRepoBase<AccountEntity>
    {
        private readonly List<AccountEntity> _accounts;

        public MockReadonlyAccountRepo(List<AccountEntity> accounts)
        {
            _accounts = accounts;
        }

        protected override List<AccountEntity> Entities => _accounts;
    }
}
