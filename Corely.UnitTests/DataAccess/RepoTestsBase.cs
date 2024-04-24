using AutoFixture;
using Corely.Domain.Entities;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess
{
    public abstract class RepoTestsBase<T>
        where T : IHasIdPk
    {
        protected readonly Fixture fixture = new();
        protected abstract IRepo<T> Repo { get; }


        [Fact]
        public async Task Create_ThenGet_ShouldReturnAdded()
        {
            var entity = fixture.Create<T>();

            await Repo.CreateAsync(entity);
            var result = await Repo.GetAsync(entity.Id);

            Assert.Equal(entity, result);
        }


        [Fact]
        public async Task Create_ThenUpdate_ShouldUpdate()
        {
            var entity = fixture.Create<T>();
            var updateEntity = fixture.Create<T>();
            updateEntity.Id = entity.Id;

            await Repo.CreateAsync(entity);
            await Repo.UpdateAsync(entity);
            var result = await Repo.GetAsync(entity.Id);

            Assert.Equal(entity, result);
        }

        [Fact]
        public async Task Create_ThenDelete_ShouldDelete()
        {
            var entity = fixture.Create<T>();

            await Repo.CreateAsync(entity);
            await Repo.DeleteAsync(entity);
            var result = await Repo.GetAsync(entity.Id);

            Assert.Null(result);
        }
    }
}
