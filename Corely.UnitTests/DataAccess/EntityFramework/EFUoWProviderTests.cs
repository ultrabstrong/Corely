using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.IAM;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Corely.UnitTests.DataAccess.EntityFramework
{
    public class EFUoWProviderTests
    {
        private readonly Mock<IAMDbContext> _iamDbContextMock;
        private readonly EFUoWProvider _efUoWProvider;

        public EFUoWProviderTests()
        {
            _iamDbContextMock = GetMockIAMDbContext();
            _efUoWProvider = new(_iamDbContextMock.Object);
        }

        private static Mock<IAMDbContext> GetMockIAMDbContext()
        {
            var iamDbContextMock = new Mock<IAMDbContext>(new EFConfigurationFixture());
            var mockDatabaseFacade = new Mock<DatabaseFacade>(iamDbContextMock.Object);
            iamDbContextMock.SetupGet(c => c.Database).Returns(mockDatabaseFacade.Object);

            return iamDbContextMock;
        }

        [Fact]
        public async Task BeginAsync_BeginsTransaction()
        {
            await _efUoWProvider.BeginAsync();

            _iamDbContextMock.Verify(c =>
                c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CommitAsync_CommitsTransaction()
        {
            await _efUoWProvider.CommitAsync();

            _iamDbContextMock.Verify(c =>
                c.Database.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RollbackAsync_RollsbackTransaction()
        {
            await _efUoWProvider.RollbackAsync();

            _iamDbContextMock.Verify(c =>
                c.Database.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_DisposesDbContext()
        {
            _efUoWProvider.Dispose();

            _iamDbContextMock.Verify(c => c.Dispose(), Times.Once);
        }
    }
}
