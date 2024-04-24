using AutoFixture;
using Corely.Domain.Entities;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess
{
    public abstract class RepoExtendedGetTestsBase<T>
        : RepoTestsBase<T>
        where T : IHasIdPk
    {
        protected abstract override IRepoExtendedGet<T> Repo { get; }

        [Fact]
        public async Task Create_ThenExtendedGetById_ShouldReturnAddedEntity()
        {
            var entity = fixture.Create<T>();

            await Repo.CreateAsync(entity);
            var result = await Repo.GetAsync(u => u.Id == entity.Id);

            Assert.Equal(entity, result);
        }
    }
}
