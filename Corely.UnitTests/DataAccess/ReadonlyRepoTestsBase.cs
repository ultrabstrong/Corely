using AutoFixture;
using Corely.Domain.Entities;
using Corely.Domain.Repos;

namespace Corely.UnitTests.DataAccess
{
    public abstract class ReadonlyRepoTestsBase<T>
        where T : class, IHasIdPk
    {
        protected readonly Fixture fixture = new();

        protected abstract IReadonlyRepo<T> Repo { get; }

        protected abstract int GetId { get; }

        [Fact]
        public async Task GetAsync_ShouldReturnEntity_WhenEntityExists()
        {
            var result = await Repo.GetAsync(GetId);

            Assert.NotNull(result);
            Assert.Equal(GetId, result.Id);
        }
    }
}
