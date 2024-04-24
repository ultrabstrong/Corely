using AutoFixture;
using Corely.DataAccess.Repos;
using Corely.Domain.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Repos
{
    public class MockReadonlyRepoBaseTests : ReadonlyRepoTestsBase<EntityFixture>
    {

        private readonly MockReadonlyRepo<EntityFixture> _mockReadonlyRepo;
        private readonly int _getId;

        public MockReadonlyRepoBaseTests()
        {
            var mockRepo = new MockRepo<EntityFixture>();

            var entityList = fixture.CreateMany<EntityFixture>(5).ToList();
            foreach (var entity in entityList)
            {
                mockRepo.CreateAsync(entity);
            }

            _mockReadonlyRepo = new MockReadonlyRepo<EntityFixture>(mockRepo);
            _getId = entityList[2].Id;
        }


        protected override IReadonlyRepo<EntityFixture> Repo => _mockReadonlyRepo;

        protected override int GetId => _getId;
    }
}
