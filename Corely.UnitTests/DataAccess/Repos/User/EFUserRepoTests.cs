using AutoFixture;
using Corely.DataAccess.DataSources.EntityFramework;
using Corely.DataAccess.Repos.User;
using Corely.Domain.Repos;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.Repos.User
{
    [Collection(CollectionNames.ServiceFactory)]
    public class EFUserRepoTests : UserRepoTestsBase
    {
        private readonly EFUserRepo _mockEFUserRepo;

        protected override IUserRepo MockUserRepo => _mockEFUserRepo;

        public EFUserRepoTests(ServiceFactory serviceFactory)
        {
            var options = new DbContextOptionsBuilder<AccountManagementDbContext>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            _mockEFUserRepo = new EFUserRepo(
                serviceFactory.GetRequiredService<ILogger<EFUserRepo>>(),
                new AccountManagementDbContext(options));
        }
    }
}
