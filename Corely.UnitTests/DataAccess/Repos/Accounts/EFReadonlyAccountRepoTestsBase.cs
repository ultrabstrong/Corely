using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Accounts;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.Accounts
{
    public class EFReadonlyAccountRepoTestsBase : AccountReadonlyRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFReadonlyAccountRepo _efReadonlyAccountRepo;
        private readonly int _getId;

        public EFReadonlyAccountRepoTestsBase()
        {
            var entityList = fixture.CreateMany<AccountEntity>(5).ToList();
            _efReadonlyAccountRepo = CreateEFReadonlyAccountRepo(entityList);
            _getId = entityList[2].Id;
        }

        protected override IReadonlyRepo<AccountEntity> Repo => _efReadonlyAccountRepo;

        protected override int GetId => _getId;

        private EFReadonlyAccountRepo CreateEFReadonlyAccountRepo(IEnumerable<AccountEntity> accountEntities)
        {
            var accountManagementDbContext = new AccountManagementDbContext(new EFConfigurationFixture());
            accountManagementDbContext.Accounts.AddRange(accountEntities);

            return new EFReadonlyAccountRepo(
                _serviceFactory.GetRequiredService<ILogger<EFReadonlyAccountRepo>>(),
                accountManagementDbContext);
        }
    }
}
