using Corely.Security;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Security
{
    public class AesKeyProviderTests
    {
        private readonly AesKeyProvider _aesKeyProvider;

        public AesKeyProviderTests()
        {
            _aesKeyProvider = new AesKeyProvider();
        }

        [Fact]
        public void GetKey_ShouldReturnValidKey()
        {
            var key = _aesKeyProvider.GetKey();
            Assert.True(_aesKeyProvider.IsKeyValid(key));
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void IsKeyValid_ShouldReturnFalse_WhenKeyNotProvided(string key)
        {
            Assert.False(_aesKeyProvider.IsKeyValid(key));
        }

        [Fact]
        public void IsKeyValid_ShouldReturnFalse_WhenKeyIsInvalid()
        {
            Assert.False(_aesKeyProvider.IsKeyValid("invalid key"));
        }
    }
}
