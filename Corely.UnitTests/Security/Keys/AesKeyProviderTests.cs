using Corely.Security.Keys;
using Corely.UnitTests.ClassData;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Keys
{
    public class AesKeyProviderTests
    {
        private readonly AesKeyProvider _aesKeyProvider = new();

        [Fact]
        public void GetKey_ReturnsValidKeyKey()
        {
            var key = _aesKeyProvider.CreateKey();
            using (Aes aes = Aes.Create())
            {
                try
                {
                    aes.Key = Convert.FromBase64String(key);
                }
                catch (Exception)
                {
                    Assert.Fail("Aes key invalid");
                }
            }
        }

        [Fact]
        public void IsKeyValid_ReturnsTrue_WithKeyFromCreateKey()
        {
            var key = _aesKeyProvider.CreateKey();
            Assert.True(_aesKeyProvider.IsKeyValid(key));
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void IsKeyValid_ReturnsFalse_WithNullOrWhitespaceKey(string key)
        {
            Assert.False(_aesKeyProvider.IsKeyValid(key));
        }

        [Fact]
        public void IsKeyValid_ReturnsFalseForInvalidKey()
        {
            var isValid = _aesKeyProvider.IsKeyValid("asdf");
            Assert.False(isValid);
        }
    }
}
