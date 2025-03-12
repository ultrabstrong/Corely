using Corely.DataAccess.Mock;

namespace Corely.DataAccess.UnitTests.Mock;

public class MockUoWProviderTests
{
    private readonly MockUoWProvider _mockUoWProvider = new();

    [Fact]
    public async Task BeginAsync_ReturnsCompletedTask()
    {
        await _mockUoWProvider.BeginAsync();

        Assert.True(true);
    }

    [Fact]
    public async Task CommitAsync_ReturnsCompletedTask()
    {
        await _mockUoWProvider.CommitAsync();

        Assert.True(true);
    }

    [Fact]
    public async Task RollbackAsync_ReturnsCompletedTask()
    {
        await _mockUoWProvider.RollbackAsync();

        Assert.True(true);
    }
}
