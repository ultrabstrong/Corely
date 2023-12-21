using Corely.Shared.Models.Security;
using Corely.Shared.Providers.Security;

namespace Corely.UnitTests.Shared.Models.Security
{
    public class EncryptedValueTests
    {
        private readonly EncryptedValue _encryptedValue;

        public EncryptedValueTests()
        {
            var keyProvider = new AesKeyProvider();
            var secretProvider = new InMemorySecretProvider(keyProvider.CreateKey());
            var encryptionProvider = new AESEncryptionProvider(secretProvider);

            _encryptedValue = new EncryptedValue(encryptionProvider);
        }

        [Fact]
        public void SetAndGet_ShouldReturnOriginalValue()
        {
            var originalValue = "This is a test";
            _encryptedValue.Set(originalValue);
            var decryptedValue = _encryptedValue.Get();
            Assert.Equal(originalValue, decryptedValue);
        }
    }
}
