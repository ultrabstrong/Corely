using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    public class EFBasicAuthRepoTests : BasicAuthRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory = new();
        private readonly EFBasicAuthRepo _efBasicAuthRepo;

        protected override IRepoExtendedGet<BasicAuthEntity> Repo => _efBasicAuthRepo;

        public EFBasicAuthRepoTests()
        {
            _efBasicAuthRepo = CreateEFBasicAuthRepo();
        }

        [Fact]
        public async Task Dispose_ShouldDisposeDbContext()
        {
            var mockEFBasicAuthRepo = CreateEFBasicAuthRepo();
            var basicAuth = fixture.Create<BasicAuthEntity>();

            mockEFBasicAuthRepo.Dispose();
            var ex = await Record.ExceptionAsync(() => mockEFBasicAuthRepo.CreateAsync(basicAuth));

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }

        private EFBasicAuthRepo CreateEFBasicAuthRepo()
        {
            return new EFBasicAuthRepo(
                _serviceFactory.GetRequiredService<ILogger<EFBasicAuthRepo>>(),
                new AccountManagementDbContext(new EFConfigurationFixture()));
        }
    }
}
