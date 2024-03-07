using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Accounts;
using Corely.Domain.Entities.Accounts;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.Accounts
{
    public class EFAccountRepoTests : AccountRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFAccountRepo _efAccountRepo;

        protected override IRepoExtendedGet<AccountEntity> Repo => _efAccountRepo;

        public EFAccountRepoTests()
        {
            _efAccountRepo = CreateEfAccountRepo();
        }

        [Fact]
        public async Task Dispose_ShouldDisposeDbContext()
        {
            var mockEFAccountAuthRepo = CreateEfAccountRepo();
            var account = fixture.Create<AccountEntity>();

            mockEFAccountAuthRepo.Dispose();
            var ex = await Record.ExceptionAsync(() => mockEFAccountAuthRepo.CreateAsync(account));

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }

        private EFAccountRepo CreateEfAccountRepo()
        {
            return new EFAccountRepo(
                _serviceFactory.GetRequiredService<ILogger<EFAccountRepo>>(),
                new AccountManagementDbContext(new EFConfigurationFixture()));
        }

    }
}
