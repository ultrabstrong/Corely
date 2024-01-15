using AutoFixture;
using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class ERepoExtendedGetBaseTests : RepoExtendedGetTestsBase<EntityFixture>
    {
        private class TestEFExtendedGetRepo : EFRepoExtendedGetBase<EntityFixture>
        {
            private readonly DbContextFixture _dbContext;

            protected override DbContext DbContext => _dbContext;
            protected override DbSet<EntityFixture> Entities => _dbContext.Entities;

            public TestEFExtendedGetRepo()
            {
                var fixture = new Fixture();
                var options = new DbContextOptionsBuilder<DbContextFixture>()
                .UseInMemoryDatabase(databaseName: fixture.Create<string>())
                .Options;

                _dbContext = new DbContextFixture(options);
            }
        }

        private readonly TestEFExtendedGetRepo _efExtendedGetRepoFixture = new();
        protected override IRepoExtendedGet<EntityFixture> Repo => _efExtendedGetRepoFixture;
    }
}
