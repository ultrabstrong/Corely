using Corely.Security.Keys;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Security.Keys
{
    public class RandomKeyProviderTests
    {
        private readonly RandomKeyProvider _hmacSha256KeyProvider = new();

        [Fact]
        public void Constructor_UseDefaultKeySize()
        {
            var keyProvider = new RandomKeyProvider();
            var key = keyProvider.CreateKey();
            var keyBytes = Convert.FromBase64String(key);
            Assert.Equal(RandomKeyProvider.DEFAULT_KEY_SIZE, keyBytes.Length);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_Throws_WithInvalidKeySize(int keySize)
        {
            var ex = Record.Exception(() => new RandomKeyProvider(keySize));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        [Fact]
        public void GetKey_ReturnsValidKeyKey()
        {
            var key = _hmacSha256KeyProvider.CreateKey();

            var keyBytes = Convert.FromBase64String(key);
            Assert.Equal(RandomKeyProvider.DEFAULT_KEY_SIZE, keyBytes.Length);
        }

        [Fact]
        public void GetKey_UsesKeyLength_FromConstructor()
        {
            var keySize = 64;
            var keyProvider = new RandomKeyProvider(keySize);
            var key = keyProvider.CreateKey();
            var keyBytes = Convert.FromBase64String(key);
            Assert.Equal(keySize, keyBytes.Length);
        }

        [Fact]
        public void IsKeyValid_ReturnsTrue_WithKeyFromCreateKey()
        {
            var key = _hmacSha256KeyProvider.CreateKey();
            Assert.True(_hmacSha256KeyProvider.IsKeyValid(key));
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void IsKeyValid_ReturnsFalse_WithNullOrWhitespaceKey(string key)
        {
            Assert.False(_hmacSha256KeyProvider.IsKeyValid(key));
        }

        [Fact]
        public void IsKeyValid_ReturnsFalseForInvalidKey()
        {
            var isValid = _hmacSha256KeyProvider.IsKeyValid("asdf");
            Assert.False(isValid);
        }
    }
}
