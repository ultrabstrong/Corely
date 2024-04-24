using Corely.DataAccess.EntityFramework.Repos;
using Microsoft.EntityFrameworkCore;

namespace Corely.UnitTests.DataAccess.EntityFramework.Repos
{
    public class DbSetExtendedGetExtensionsTests
    {
        private readonly Mock<DbSet<object>> _dbSet;

        public DbSetExtendedGetExtensionsTests()
        {
            _dbSet = new Mock<DbSet<object>>();
        }

        [Fact]
        public async Task GetAsync_ShouldThrowArgumentNullException_WithNullDbSet()
        {
            var ex = await Record.ExceptionAsync(() => ((DbSet<object>?)null).GetAsync(null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowArgumentNullException_WithNullQuery()
        {
            var ex = await Record.ExceptionAsync(() => _dbSet.Object.GetAsync(null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
