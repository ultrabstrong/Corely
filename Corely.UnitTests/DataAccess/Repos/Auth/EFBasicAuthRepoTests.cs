using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Corely.UnitTests.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    [Collection(CollectionNames.ServiceFactory)]
    public class EFBasicAuthRepoTests : BasicAuthRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly EFBasicAuthRepo _mockEFBasicAuthRepo;

        protected override IAuthRepo<BasicAuthEntity> MockBasicAuthRepo => _mockEFBasicAuthRepo;

        public EFBasicAuthRepoTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _mockEFBasicAuthRepo = CreateEfBasicAuthRepo();
        }

        [Fact]
        public async Task Dispose_ShouldDisposeDbContext()
        {
            var mockEFBasicAuthRepo = CreateEfBasicAuthRepo();
            var basicAuth = fixture.Create<BasicAuthEntity>();

            mockEFBasicAuthRepo.Dispose();
            var ex = await Record.ExceptionAsync(() => mockEFBasicAuthRepo.Create(basicAuth));

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }

        private EFBasicAuthRepo CreateEfBasicAuthRepo()
        {
            var options = new DbContextOptionsBuilder<AccountManagementDbContext>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            return new EFBasicAuthRepo(
                _serviceFactory.GetRequiredService<ILogger<EFBasicAuthRepo>>(),
                new AccountManagementDbContext(options));
        }
    }
}
