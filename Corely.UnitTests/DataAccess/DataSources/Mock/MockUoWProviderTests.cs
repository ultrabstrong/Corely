using Corely.DataAccess.DataSources.Mock;

namespace Corely.UnitTests.DataAccess.DataSources.Mock
{
    public class MockUoWProviderTests
    {
        private readonly MockUoWProvider _mockUoWProvider = new();

        [Fact]
        public async Task BeginAsync_ShouldReturnCompletedTask()
        {
            await _mockUoWProvider.BeginAsync();

            Assert.True(true);
        }

        [Fact]
        public async Task CommitAsync_ShouldReturnCompletedTask()
        {
            await _mockUoWProvider.CommitAsync();

            Assert.True(true);
        }

        [Fact]
        public async Task RollbackAsync_ShouldReturnCompletedTask()
        {
            await _mockUoWProvider.RollbackAsync();

            Assert.True(true);
        }
    }
}
