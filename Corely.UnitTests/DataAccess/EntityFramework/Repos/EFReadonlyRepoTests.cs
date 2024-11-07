using AutoFixture;
using Corely.DataAccess.EntityFramework.Repos;
using Corely.DataAccess.Interfaces.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class EFReadonlyRepoTests : ReadonlyRepoTestsBase<EntityFixture>
    {
        private readonly EFReadonlyRepo<EntityFixture> _efReadonlyRepo;
        private readonly int _getId;

        public EFReadonlyRepoTests()
        {
            var serviceFactory = new ServiceFactory();
            var dbContext = GetDbContext();
            var dbSet = dbContext.Set<EntityFixture>();

            _efReadonlyRepo = new(
                serviceFactory.GetRequiredService<ILogger<EFReadonlyRepo<EntityFixture>>>(),
                dbContext);

            _getId = dbSet.Skip(1).First().Id;
        }

        private static DbContextFixture GetDbContext()
        {
            var fixture = new Fixture();
            var options = new DbContextOptionsBuilder<DbContextFixture>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

            var dbContext = new DbContextFixture(options);

            var entityList = fixture.CreateMany<EntityFixture>(5).ToList();
            foreach (var entity in entityList)
            {
                dbContext.Entities.Add(entity);
            }
            dbContext.SaveChanges();

            return dbContext;
        }

        protected override IReadonlyRepo<EntityFixture> Repo => _efReadonlyRepo;

        protected override int GetId => _getId;
    }
}
