using AutoFixture;
using Corely.Domain.Entities.Accounts;

namespace Corely.UnitTests.DataAccess.Repos.Accounts
{
    public abstract class AccountRepoTestsBase : RepoExtendedGetTestsBase<AccountEntity>
    {
        public AccountRepoTestsBase()
        {
            fixture.Customize<AccountEntity>(c => c
                .Without(e => e.Users));
        }

        [Fact]
        public async Task Create_ThenGetByName_ShouldReturnAddedAccount()
        {
            var account = fixture.Create<AccountEntity>();

            await Repo.CreateAsync(account);
            var result = await Repo.GetAsync(a => a.AccountName == account.AccountName);

            Assert.Equal(account, result);
        }
    }
}
