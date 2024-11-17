using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.DataAccess.Mock.Repos;
using Corely.UnitTests.Fixtures;

namespace Corely.UnitTests.DataAccess.Mock.Repos
{
    public class MockRepoTests : RepoTestsBase
    {
        private readonly MockRepo<EntityFixture> _mockRepo = new();
        protected override IRepo<EntityFixture> Repo => _mockRepo;
        protected override int FillRepoAndReturnId()
        {
            var entityList = Fixture.CreateMany<EntityFixture>(5).ToList();
            foreach (var entity in entityList)
            {
                _mockRepo.CreateAsync(entity);
            }

            return entityList[2].Id;
        }
    }
}
