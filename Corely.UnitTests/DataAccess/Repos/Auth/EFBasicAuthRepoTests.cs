using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    [Collection(CollectionNames.ServiceFactory)]
    public class EFBasicAuthRepoTests : BasicAuthRepoTestsBase
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly EFBasicAuthRepo _efBasicAuthRepo;

        protected override IRepoExtendedGet<BasicAuthEntity> Repo => _efBasicAuthRepo;

        public EFBasicAuthRepoTests(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _efBasicAuthRepo = CreateEfBasicAuthRepo();
        }

        [Fact]
        public async Task Dispose_ShouldDisposeDbContext()
        {
            var mockEFBasicAuthRepo = CreateEfBasicAuthRepo();
            var basicAuth = fixture.Create<BasicAuthEntity>();

            mockEFBasicAuthRepo.Dispose();
            var ex = await Record.ExceptionAsync(() => mockEFBasicAuthRepo.CreateAsync(basicAuth));

            Assert.NotNull(ex);
            Assert.IsType<ObjectDisposedException>(ex);
        }

        private EFBasicAuthRepo CreateEfBasicAuthRepo()
        {
            return new EFBasicAuthRepo(
                _serviceFactory.GetRequiredService<ILogger<EFBasicAuthRepo>>(),
                new AccountManagementDbContext(new EFConfigurationFixture()));
        }
    }
}
