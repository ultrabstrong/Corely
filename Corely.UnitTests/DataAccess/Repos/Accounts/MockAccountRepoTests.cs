using Corely.DataAccess.Repos.Accounts;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.Accounts
{
    public class MockAccountRepoTests : AccountRepoTestsBase
    {
        private readonly MockAccountRepo _mockAccountRepo = new();
        protected override IRepoExtendedGet<AccountEntity> Repo => _mockAccountRepo;
    }
}
