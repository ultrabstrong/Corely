using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.IAM.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class EFRepoTests : RepoTestsBase<EntityFixture>
    {
        private readonly EFRepo<EntityFixture> _efRepo;

        public EFRepoTests()
        {
            var serviceFactory = new ServiceFactory();
            var dbContext = GetDbContext();

            _efRepo = new EFRepo<EntityFixture>(
                serviceFactory.GetRequiredService<ILogger<EFRepo<EntityFixture>>>(),
                () => dbContext.SaveChangesAsync(),
                dbContext.Entities);
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

        protected override IRepo<EntityFixture> Repo => _efRepo;
    }
}
