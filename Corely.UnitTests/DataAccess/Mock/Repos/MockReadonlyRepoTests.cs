using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Mock.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos
{
    public class MockReadonlyRepoTests : ReadonlyRepoTestsBase
    {

        private readonly MockReadonlyRepo<EntityFixture> _mockReadonlyRepo;
        private readonly int _getId;

        public MockReadonlyRepoTests()
        {
            var mockRepo = new MockRepo<EntityFixture>();

            var entityList = Fixture.CreateMany<EntityFixture>(5).ToList();
            foreach (var entity in entityList)
            {
                mockRepo.CreateAsync(entity);
            }

            _mockReadonlyRepo = new MockReadonlyRepo<EntityFixture>(mockRepo);
            _getId = entityList[2].Id;
        }


        protected override IReadonlyRepo<EntityFixture> ReadonlyRepo => _mockReadonlyRepo;

        protected override int GetId => _getId;
    }
}
