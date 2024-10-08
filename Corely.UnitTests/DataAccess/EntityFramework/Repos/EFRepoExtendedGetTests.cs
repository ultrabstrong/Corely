using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.IAM.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class EFRepoExtendedGetTests : RepoExtendedGetTestsBase
    {
        private readonly EFRepoExtendedGet<EntityFixture> _efExtendedGetRepo;
        private readonly DbContextFixture _dbContext;

        public EFRepoExtendedGetTests()
        {
            var serviceFactory = new ServiceFactory();
            _dbContext = GetDbContext();

            _efExtendedGetRepo = new EFRepoExtendedGet<EntityFixture>(
                serviceFactory.GetRequiredService<ILogger<EFRepoExtendedGet<EntityFixture>>>(),
                () => _dbContext.SaveChangesAsync(),
                _dbContext.Entities);
        }

        private static DbContextFixture GetDbContext()
        {
            var fixture = new Fixture();
            var options = new DbContextOptionsBuilder<DbContextFixture>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            var dbContext = new DbContextFixture(options);

            return dbContext;
        }

        protected override IRepoExtendedGet<EntityFixture> Repo => _efExtendedGetRepo;
    }
}
