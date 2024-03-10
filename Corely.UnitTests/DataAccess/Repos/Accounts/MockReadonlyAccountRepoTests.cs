using AutoFixture;
using Corely.DataAccess.Repos.Accounts;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess.Repos.Accounts
{
    public class MockReadonlyAccountRepoTests : AccountReadonlyRepoTestsBase
    {
        private readonly MockReadonlyAccountRepo _mockReadonlyAccountRepo;
        private readonly int _getId;

        public MockReadonlyAccountRepoTests()
        {
            var mockAccountRepo = new MockAccountRepo();
            var entityList = fixture.CreateMany<AccountEntity>(5).ToList();
            foreach (var entity in entityList)
            {
                mockAccountRepo.CreateAsync(entity);
            }
            _mockReadonlyAccountRepo = new MockReadonlyAccountRepo(mockAccountRepo);
            _getId = entityList[2].Id;
        }

        protected override IReadonlyRepo<AccountEntity> Repo => _mockReadonlyAccountRepo;

        protected override int GetId => _getId;
    }
}
