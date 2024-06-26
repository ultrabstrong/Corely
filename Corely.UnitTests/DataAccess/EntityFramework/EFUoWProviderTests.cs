﻿using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.EntityFramework.IAM;
using Corely.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Corely.UnitTests.DataAccess.EntityFramework
{
    public class EFUoWProviderTests
    {
        private readonly Mock<IAMDbContext> _accountManagementDbContextMock;
        private readonly EFUoWProvider _efUoWProvider;

        public EFUoWProviderTests()
        {
            _accountManagementDbContextMock = GetMockAccountManagementDbContext();
            _efUoWProvider = new(_accountManagementDbContextMock.Object);
        }

        private static Mock<IAMDbContext> GetMockAccountManagementDbContext()
        {
            var accountManagementDbContextMock = new Mock<IAMDbContext>(new EFConfigurationFixture());
            var mockDatabaseFacade = new Mock<DatabaseFacade>(accountManagementDbContextMock.Object);
            accountManagementDbContextMock.SetupGet(c => c.Database).Returns(mockDatabaseFacade.Object);

            return accountManagementDbContextMock;
        }

        [Fact]
        public async Task BeginAsync_BeginsTransaction()
        {
            await _efUoWProvider.BeginAsync();

            _accountManagementDbContextMock.Verify(c =>
                c.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CommitAsync_CommitsTransaction()
        {
            await _efUoWProvider.CommitAsync();

            _accountManagementDbContextMock.Verify(c =>
                c.Database.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RollbackAsync_RollsbackTransaction()
        {
            await _efUoWProvider.RollbackAsync();

            _accountManagementDbContextMock.Verify(c =>
                c.Database.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Dispose_DisposesDbContext()
        {
            _efUoWProvider.Dispose();

            _accountManagementDbContextMock.Verify(c => c.Dispose(), Times.Once);
        }
    }
}
