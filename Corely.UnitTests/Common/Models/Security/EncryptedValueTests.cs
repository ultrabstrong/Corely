using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Keys;

namespace Corely.UnitTests.Common.Models.Security
{
    public class EncryptedValueTests
    {
        private readonly EncryptedValue _encryptedValue;

        public EncryptedValueTests()
        {
            var keyProvider = new AesKeyProvider();
            var keyStoreProvider = new InMemoryKeyStoreProvider(keyProvider.CreateKey());
            var encryptionProvider = new AesEncryptionProvider(keyStoreProvider);

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
