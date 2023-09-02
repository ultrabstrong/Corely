using Corely.Shared.Providers.Data;

namespace Corely.UnitTests.Shared.Providers.Data
{
    public class RandomStringProviderTests
    {
        private readonly RandomStringProvider _randomStringProvider = new();

        [Fact]
        public void GetString_ShouldThrowException_WhenLengthIsNegative()
        {
            var length = -1;
            void act() => _randomStringProvider.GetString(length);
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void GetString_ShouldReturnStringOfSpecifiedLength()
        {
            var length = 10;
            var result = _randomStringProvider.GetString(length);
            Assert.Equal(length, result.Length);
        }

        [Fact]
        public void GetString_ShouldReturnStringOfSpecifiedLength_WhenLengthIsZero()
        {
            var length = 0;
            var result = _randomStringProvider.GetString(length);
            Assert.Equal(length, result.Length);
        }

    }
}
