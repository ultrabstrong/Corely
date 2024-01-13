using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.Auth;
using Corely.Domain.Entities.Auth;
using Corely.Domain.Repos;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.Auth
{
    [Collection(CollectionNames.ServiceFactory)]
    public class EFBasicAuthRepoTests : BasicAuthRepoTestsBase
    {
        private readonly EFBasicAuthRepo _mockEFBasicAuthRepo;

        protected override IAuthRepo<BasicAuthEntity> MockBasicAuthRepo => _mockEFBasicAuthRepo;

        public EFBasicAuthRepoTests(ServiceFactory serviceFactory)
        {
            var options = new DbContextOptionsBuilder<AccountManagementDbContext>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            _mockEFBasicAuthRepo = new EFBasicAuthRepo(
                serviceFactory.GetRequiredService<ILogger<EFBasicAuthRepo>>(),
                new AccountManagementDbContext(options));
        }
    }
}
