using Corely.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.Extensions
{
    public class DbSetExtensionsTests
    {
        private readonly Mock<DbSet<object>> _dbSet;

        public DbSetExtensionsTests()
        {
            _dbSet = new Mock<DbSet<object>>();
        }

        [Fact]
        public async Task GetAsync_ShouldThrowArgumentNullException_WithNullDbSet()
        {
            var ex = await Record.ExceptionAsync(() => DbSetExtensions.GetAsync((DbSet<object>?)null, null));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowArgumentNullException_WithNullQuery()
        {
            var ex = await Record.ExceptionAsync(() => _dbSet.Object.GetAsync(null));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException);
        }
    }
}
