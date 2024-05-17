using AutoFixture;
using Corely.IAM.Entities;
using Corely.IAM.Repos;

namespace Corely.UnitTests.DataAccess
{
    public abstract class RepoExtendedGetTestsBase<T>
        : RepoTestsBase<T>
        where T : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        protected abstract override IRepoExtendedGet<T> Repo { get; }

        [Fact]
        public async Task Create_ThenExtendedGetById_ReturnsAddedEntity()
        {
            var entity = fixture.Create<T>();

            await Repo.CreateAsync(entity);
            var result = await Repo.GetAsync(u => u.Id == entity.Id);

            Assert.Equal(entity, result);
        }
    }
}
