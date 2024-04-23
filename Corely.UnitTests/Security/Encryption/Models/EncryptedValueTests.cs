using AutoFixture;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.UnitTests.Security.Encryption.Models
{
    public class EncryptedValueTests
    {
        private readonly EncryptedValue _encryptedValue;
        private readonly Fixture _fixture = new();

        public EncryptedValueTests()
        {
            var keyProvider = new AesKeyProvider();
            var keyStoreProvider = new InMemoryKeyStoreProvider(keyProvider.CreateKey());
            var encryptionProvider = new AesEncryptionProvider(keyStoreProvider);

            _encryptedValue = new EncryptedValue(encryptionProvider);
        }

        [Fact]
        public void Constructor_ShouldCreateEncryptedValue()
        {
            Assert.NotNull(_encryptedValue);
        }

        [Fact]
        public void Constructor_ShouldCreateEncryptedValueWithSecret()
        {
            var keyProvider = new AesKeyProvider();
            var keyStoreProvider = new InMemoryKeyStoreProvider(keyProvider.CreateKey());
            var encryptionProvider = new AesEncryptionProvider(keyStoreProvider);
            var value = _fixture.Create<string>();

            var encryptedValue = new EncryptedValue(encryptionProvider) { Secret = value };

            Assert.Equal(value, encryptedValue.Secret);
        }

        [Fact]
        public void Set_ShouldSetEncryptedSecret()
        {
            var value = _fixture.Create<string>();
            _encryptedValue.Set(value);
            Assert.NotNull(_encryptedValue.Secret);
            Assert.NotEmpty(_encryptedValue.Secret);
            Assert.NotEqual(value, _encryptedValue.Secret);
        }

        [Fact]
        public void Get_ShouldGetDecryptedSecret()
        {
            var value = _fixture.Create<string>();
            _encryptedValue.Set(value);
            var decryptedValue = _encryptedValue.GetDecrypted();
            Assert.Equal(value, decryptedValue);
        }

        [Fact]
        public void ReEncrypt_ShouldReEncryptSecret()
        {
            var value = _fixture.Create<string>();
            _encryptedValue.Set(value);
            var oldSecret = _encryptedValue.Secret;
            _encryptedValue.ReEncrypt();
            Assert.NotEqual(oldSecret, _encryptedValue.Secret);
        }
    }
}
