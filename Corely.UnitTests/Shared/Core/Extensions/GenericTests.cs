using Corely.Shared.Core.Extensions;

namespace Corely.UnitTests.Core.Extensions
{
    public class GenericTests
    {
        [Fact]
        public void ThrowIfNull_ShouldThrow_WhenNull()
        {
            object? obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull());
        }

        [Fact]
        public void ThrowIfNull_ShouldReturnObject_WhenNotNull()
        {
            object obj = new();
            Assert.Equal(obj.ThrowIfNull(), obj);
        }
    }
}
