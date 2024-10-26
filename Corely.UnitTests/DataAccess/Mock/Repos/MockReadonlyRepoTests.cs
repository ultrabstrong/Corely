using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Mock.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos
{
    public class MockReadonlyRepoTests : ReadonlyRepoTestsBase<EntityFixture>
    {

        private readonly MockReadonlyRepo<EntityFixture> _mockReadonlyRepo;
        private readonly int _getId;

        public MockReadonlyRepoTests()
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
