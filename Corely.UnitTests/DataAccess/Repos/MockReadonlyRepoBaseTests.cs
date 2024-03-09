using AutoFixture;
using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class MockReadonlyRepoBaseTests : ReadonlyRepoTestsBase<EntityFixture>
    {
        private class MockReadonlyRepo : MockReadonlyRepoBase<EntityFixture>
        {
            private readonly List<EntityFixture> _entities;

            public MockReadonlyRepo(List<EntityFixture> entities)
            {
                _entities = entities;
            }

            protected override List<EntityFixture> Entities => _entities;
        }

        private readonly MockReadonlyRepo _mockReadonlyRepo;
        private readonly int _getId;

        public MockReadonlyRepoBaseTests()
        {
            var entityList = fixture.CreateMany<EntityFixture>(5).ToList();
            _mockReadonlyRepo = new MockReadonlyRepo(entityList);
            _getId = entityList[2].Id;
        }


        protected override IReadonlyRepo<EntityFixture> Repo => _mockReadonlyRepo;

        protected override int GetId => _getId;
    }
}
