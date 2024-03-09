using AutoFixture;
using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class EFReadonlyRepoBaseTests : ReadonlyRepoTestsBase<EntityFixture>
    {
        private class TestEFReadonlyRepo : EFReadonlyRepoBase<EntityFixture>
        {
            private readonly DbContextFixture _dbContext;

            protected override DbContext DbContext => _dbContext;
            protected override DbSet<EntityFixture> Entities => _dbContext.Entities;

            public TestEFReadonlyRepo(IEnumerable<EntityFixture> entityFixtures)
            {
                var fixture = new Fixture();
                var options = new DbContextOptionsBuilder<DbContextFixture>()
                    .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                    .Options;

                _dbContext = new DbContextFixture(options);
                _dbContext.AddRange(entityFixtures);
            }
        }

        private readonly TestEFReadonlyRepo _efReadonlyRepoFixture;
        private readonly int _getId;

        public EFReadonlyRepoBaseTests()
        {
            var entityList = fixture.CreateMany<EntityFixture>(5).ToList();
            _efReadonlyRepoFixture = new(entityList);
            _getId = entityList[2].Id;
        }

        protected override IReadonlyRepo<EntityFixture> Repo => _efReadonlyRepoFixture;

        protected override int GetId => _getId;
    }
}
