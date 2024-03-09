using Corely.Domain.Entities.Accounts;

namespace Corely.UnitTests.DataAccess.Repos.Accounts
{
    public abstract class AccountReadonlyRepoTestsBase : ReadonlyRepoTestsBase<AccountEntity>
    {
        public AccountReadonlyRepoTestsBase()
        {
            fixture.Customize<AccountEntity>(c => c
                .Without(e => e.Users));
        }
    }
}
