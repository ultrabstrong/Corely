using AutoFixture;
using Corely.IAM.Entities;
using Corely.IAM.Repos;

namespace Corely.UnitTests.DataAccess
{
    public abstract class ReadonlyRepoTestsBase<T>
        where T : class, IHasIdPk
    {
        protected readonly Fixture fixture = new();

        protected abstract IReadonlyRepo<T> Repo { get; }

        protected abstract int GetId { get; }

        [Fact]
        public async Task GetAsync_ReturnsEntity_WhenEntityExists()
        {
            var result = await Repo.GetAsync(GetId);

            Assert.NotNull(result);
            Assert.Equal(GetId, result.Id);
        }
    }
}
