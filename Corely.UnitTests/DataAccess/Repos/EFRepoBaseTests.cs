using AutoFixture;
using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class EFRepoBaseTests : RepoTestsBase<EntityFixture>
    {
        private class TestEFRepo : EFRepoBase<EntityFixture>
        {
            private readonly DbContextFixture _dbContext;

            protected override DbContext DbContext => _dbContext;
            protected override DbSet<EntityFixture> Entities => _dbContext.Entities;

            public TestEFRepo()
            {
                var fixture = new Fixture();
                var options = new DbContextOptionsBuilder<DbContextFixture>()
                    .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                    .Options;

                _dbContext = new DbContextFixture(options);
            }
        }

        private readonly TestEFRepo _efRepoFixture = new();
        protected override IRepo<EntityFixture> Repo => _efRepoFixture;
    }
}
