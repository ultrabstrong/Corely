using Corely.DataAccess.EntityFramework;
using Corely.DataAccess.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace Corely.DataAccess.UnitTests.EntityFramework;

public class EFUoWProviderTests
{
    private readonly Mock<IDbContextTransaction> _transaction = new();
    private readonly Mock<DbContextFixture> _iamDbContextMock;
    private readonly EFUoWProvider _efUoWProvider;

    public EFUoWProviderTests()
    {
        _iamDbContextMock = GetMockIAMDbContext();
        _efUoWProvider = new(_iamDbContextMock.Object);
    }

    private Mock<DbContextFixture> GetMockIAMDbContext()
    {
        var dbContextMock = new Mock<DbContextFixture>(new EFConfigurationFixture());
        var mockDatabaseFacade = new Mock<DatabaseFacade>(dbContextMock.Object);
        mockDatabaseFacade.Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => _transaction.Object);
        dbContextMock.SetupGet(c => c.Database).Returns(mockDatabaseFacade.Object);

        return dbContextMock;
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
        await _efUoWProvider.BeginAsync();
        await _efUoWProvider.CommitAsync();

        _transaction.Verify(m => m.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transaction.Verify(m => m.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task RollbackAsync_RollsbackTransaction()
    {
        await _efUoWProvider.BeginAsync();
        await _efUoWProvider.RollbackAsync();

        _transaction.Verify(m => m.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transaction.Verify(m => m.DisposeAsync(), Times.Once);
    }

    [Fact]
    public async Task Dispose_DisposesDbContextAndTransaction()
    {
        await _efUoWProvider.BeginAsync();

        _efUoWProvider.Dispose();

        _iamDbContextMock.Verify(c => c.Dispose(), Times.Once);
        _transaction.Verify(m => m.Dispose(), Times.Once);
    }

    [Fact]
    public async Task DisposeAsync_DisposesDbContextAndTransaction()
    {
        await _efUoWProvider.BeginAsync();

        await _efUoWProvider.DisposeAsync();

        _iamDbContextMock.Verify(c => c.DisposeAsync(), Times.Once);
        _transaction.Verify(m => m.DisposeAsync(), Times.Once);
    }
}
