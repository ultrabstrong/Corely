using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    public class EFUserRepoTests : UserRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFUserRepo _efUserRepo;

        protected override IRepoExtendedGet<UserEntity> Repo => _efUserRepo;

        public EFUserRepoTests()
        {
            _efUserRepo = CreateEFUserRepo();
        }

        [Fact]
        public async Task Dispose_ShouldDisposeDbContext()
        {
            var mockEFUserRepo = CreateEFUserRepo();
            var user = fixture.Create<UserEntity>();

            mockEFUserRepo.Dispose();
            var ex = await Record.ExceptionAsync(() => mockEFUserRepo.CreateAsync(user));

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }

        private EFUserRepo CreateEFUserRepo()
        {
            return new EFUserRepo(
            _serviceFactory.GetRequiredService<ILogger<EFUserRepo>>(),
            new AccountManagementDbContext(new EFConfigurationFixture()));
        }
    }
}
