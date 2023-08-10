using Corely.Shared.Extensions;

namespace Corely.UnitTests.Shared.Extensions
{
    public class GenericExtensionsTests
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
