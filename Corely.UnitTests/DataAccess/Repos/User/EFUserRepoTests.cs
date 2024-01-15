using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Entities.Users;
using Corely.Domain.Repos;
using Corely.UnitTests.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    [Collection(CollectionNames.ServiceFactory)]
    public class EFUserRepoTests : UserRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly EFUserRepo _mockEFUserRepo;

        protected override IUserRepo MockUserRepo => _mockEFUserRepo;

        public EFUserRepoTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _mockEFUserRepo = CreateEfUserRepo();
        }

        [Fact]
        public async Task Dispose_ShouldDisposeDbContext()
        {
            var mockEFUserRepo = CreateEfUserRepo();
            var user = fixture.Create<UserEntity>();

            mockEFUserRepo.Dispose();
            var ex = await Record.ExceptionAsync(() => mockEFUserRepo.Create(user));

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }

        private EFUserRepo CreateEfUserRepo()
        {
            var options = new DbContextOptionsBuilder<AccountManagementDbContext>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            return new EFUserRepo(
            _serviceFactory.GetRequiredService<ILogger<EFUserRepo>>(),
            new AccountManagementDbContext(options));
        }
    }
}
