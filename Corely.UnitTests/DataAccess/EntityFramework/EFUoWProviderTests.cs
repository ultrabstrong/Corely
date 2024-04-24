using Corely.DataAccess.EntityFramework;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Corely.UnitTests.DataAccess.EntityFramework
{
    public class EFUoWProviderTests
    {
        private readonly Mock<AccountManagementDbContext> _accountManagementDbContextMock;
        private readonly EFUoWProvider _efUoWProvider;

        public EFUoWProviderTests()
        {
            _accountManagementDbContextMock = GetMockAccountManagementDbContext();
            _efUoWProvider = new(_accountManagementDbContextMock.Object);
        }

        private static Mock<AccountManagementDbContext> GetMockAccountManagementDbContext()
        {
            var accountManagementDbContextMock = new Mock<AccountManagementDbContext>(new EFConfigurationFixture());
            var mockDatabaseFacade = new Mock<DatabaseFacade>(accountManagementDbContextMock.Object);
            accountManagementDbContextMock.SetupGet(c => c.Database).Returns(mockDatabaseFacade.Object);

            return accountManagementDbContextMock;
        }

        [Fact]
        public async Task BeginAsync_ShouldBeginTransaction()
        {
            await _efUoWProvider.BeginAsync();

            _accountManagementDbContextMock.Verify(c =>
                c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CommitAsync_ShouldCommitTransaction()
        {
            await _efUoWProvider.CommitAsync();

            _accountManagementDbContextMock.Verify(c =>
                c.Database.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RollbackAsync_ShouldRollbackTransaction()
        {
            await _efUoWProvider.RollbackAsync();

            _accountManagementDbContextMock.Verify(c =>
                c.Database.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_ShouldDisposeDbContext()
        {
            _efUoWProvider.Dispose();

            _accountManagementDbContextMock.Verify(c => c.Dispose(), Times.Once);
        }
    }
}
