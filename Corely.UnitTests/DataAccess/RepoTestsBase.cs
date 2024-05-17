using AutoFixture;
using Corely.IAM.Entities;
using Corely.IAM.Repos;
using FluentAssertions;

namespace Corely.UnitTests.DataAccess
{
    public abstract class RepoTestsBase<T>
        where T : IHasIdPk, IHasCreatedUtc, IHasModifiedUtc
    {
        protected readonly Fixture fixture = new();
        protected abstract IRepo<T> Repo { get; }


        [Fact]
        public async Task Create_ThenGet_ReturnsAdded()
        {
            var entity = fixture.Create<T>();

            await Repo.CreateAsync(entity);
            var result = await Repo.GetAsync(entity.Id);

            Assert.Equal(entity, result);
        }

        [Fact]
        public async Task Create_ThenUpdate_Updates()
        {
            var entity = fixture.Create<T>();
            var updateEntity = fixture.Create<T>();
            updateEntity.Id = entity.Id;
            updateEntity.CreatedUtc = entity.CreatedUtc;

            await Repo.CreateAsync(entity);
            await Repo.UpdateAsync(updateEntity);
            var result = await Repo.GetAsync(entity.Id);

            result.Should().BeEquivalentTo(updateEntity, options => options.Excluding(m => m.ModifiedUtc));
        }

        [Fact]
        public async Task Create_ThenUpdate_UpdatesModifiedUtc()
        {
            var entity = fixture.Create<T>();
            entity.ModifiedUtc = DateTime.UtcNow;
            var originalModifiedUtc = entity.ModifiedUtc;

            var updateEntity = fixture.Create<T>();
            updateEntity.Id = entity.Id;

            await Repo.CreateAsync(entity);
            await Repo.UpdateAsync(updateEntity);
            var result = await Repo.GetAsync(entity.Id);

            Assert.NotNull(result);
            Assert.True(originalModifiedUtc < updateEntity.ModifiedUtc);
        }

        [Fact]
        public async Task Create_ThenDelete_Deletes()
        {
            var entity = fixture.Create<T>();

            await Repo.CreateAsync(entity);
            await Repo.DeleteAsync(entity);
            var result = await Repo.GetAsync(entity.Id);

            Assert.Null(result);
        }
    }
}
