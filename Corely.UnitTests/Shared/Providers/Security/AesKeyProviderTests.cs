using Corely.Shared.Providers.Security;
using System.Security.Cryptography;

namespace Corely.UnitTests.Shared.Providers.Security
{
    public class AesKeyProviderTests
    {
        private readonly AesKeyProvider _aesKeyProvider;

        public AesKeyProviderTests()
        {
            _aesKeyProvider = new AesKeyProvider();
        }

        [Fact]
        public void GetKey_ShouldReturnValidKeyKey()
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
        public void IsKeyValid_ShouldReturnTrueForValidKey()
        {
            var key = _aesKeyProvider.CreateKey();
            var isValid = _aesKeyProvider.IsKeyValid(key);
            Assert.True(isValid);
        }

        [Fact]
        public void IsKeyValid_ShouldReturnFalseForInvalidKey()
        {
            var isValid = _aesKeyProvider.IsKeyValid("asdf");
            Assert.False(isValid);
        }
    }
}
