using AutoFixture;
using Corely.IAM.Repos;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess
{
    public abstract class RepoExtendedGetTestsBase : RepoTestsBase<EntityFixture>
    {
        protected abstract override IRepoExtendedGet<EntityFixture> Repo { get; }

        [Fact]
        public async Task Create_ThenExtendedGetById_ReturnsAddedEntity()
        {
            var entity = fixture.Create<EntityFixture>();

            await Repo.CreateAsync(entity);
            var result = await Repo.GetAsync(u => u.Id == entity.Id);

            Assert.Equal(entity, result);
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

            await Repo.GetAsync(
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

            await Repo.GetAsync(
                u => u.Id == 1,
                include: includeMock.Object);

            includeMock.Verify(
                m => m(It.IsAny<IQueryable<EntityFixture>>()),
                Times.Once);
        }

    }
}
