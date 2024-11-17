using AutoFixture;
using Corely.DataAccess.Interfaces.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess
{
    public abstract class ReadonlyRepoTestsBase
    {
        protected readonly Fixture Fixture = new();

        protected abstract IReadonlyRepo<EntityFixture> ReadonlyRepo { get; }

        protected abstract int GetId { get; }

        [Fact]
        public async Task GetAsync_ReturnsEntity_WIthIdLookup()
        {
            var id = GetId;
            var result = await ReadonlyRepo.GetAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetAsync_ReturnsEntity_WithVerboseIdLookup()
        {
            var id = GetId;
            var result = await ReadonlyRepo.GetAsync(u => u.Id == id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }


        [Fact]
        public async Task GetAsync_Uses_OrderBy()
        {
            var orderByMock = new Mock<Func<IQueryable<EntityFixture>, IOrderedQueryable<EntityFixture>>>();
            orderByMock
                .Setup(m => m(
                    It.IsAny<IQueryable<EntityFixture>>()))
                .Returns((IQueryable<EntityFixture> q) =>
                    q.OrderBy(u => u.Id));

            await ReadonlyRepo.GetAsync(
                u => u.Id == 1,
                orderBy: orderByMock.Object);

            orderByMock.Verify(
                m => m(It.IsAny<IQueryable<EntityFixture>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAsync_Uses_Include()
        {
            var includeMock = new Mock<Func<IQueryable<EntityFixture>, IQueryable<EntityFixture>>>();
            includeMock
                .Setup(m => m(
                    It.IsAny<IQueryable<EntityFixture>>()))
                .Returns((IQueryable<EntityFixture> q) =>
                    q.Include(u => u.NavigationProperty));

            await ReadonlyRepo.GetAsync(
                u => u.Id == 1,
                include: includeMock.Object);

            includeMock.Verify(
                m => m(It.IsAny<IQueryable<EntityFixture>>()),
                Times.Once);
        }
    }
}
