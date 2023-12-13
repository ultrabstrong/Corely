using Corely.Shared.Providers.Security;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Security
{
    public class AesEncryptionProviderTests
    {
        private readonly AESEncryptionProvider _aesEncryptionProvider;

        public AesEncryptionProviderTests()
        {
            AesKeyProvider keyProvider = new();
            InMemorySecretProvider secretProvider = new(keyProvider.CreateKey());
            _aesEncryptionProvider = new(keyProvider, secretProvider);
        }

        [Fact]
        public void Encrypt_ShouldReturnEncryptedString()
        {
            var encrypted = _aesEncryptionProvider.Encrypt("test");
            Assert.NotEqual("test", encrypted);
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Encrypt_ShouldThrow_WithNullOrEmpty(string decryptedString)
        {
            var exception = Record.Exception(() => _aesEncryptionProvider.Encrypt(decryptedString));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Fact]
        public void Encrypt_ShouldProduceDifferentEncryptedStrings()
        {
            var encrypted1 = _aesEncryptionProvider.Encrypt("test");
            var encrypted2 = _aesEncryptionProvider.Encrypt("test");
            Assert.NotEqual(encrypted1, encrypted2);
        }

        [Fact]
        public void Decrypt_ShouldReturnDecryptedString()
        {
            var encrypted = _aesEncryptionProvider.Encrypt("test");
            var decrypted = _aesEncryptionProvider.Decrypt(encrypted);
            Assert.Equal("test", decrypted);
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Decrypt_ShouldThrow_WithNullOrEmpty(string encryptedString)
        {
            var exception = Record.Exception(() => _aesEncryptionProvider.Encrypt(encryptedString));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Fact]
        public void Decrypt_ShouldProduceSameStringThatWasEncrypted()
        {
            var encrypted1 = _aesEncryptionProvider.Encrypt("test");
            var encrypted2 = _aesEncryptionProvider.Encrypt("test");
            var decrypted1 = _aesEncryptionProvider.Decrypt(encrypted1);
            var decrypted2 = _aesEncryptionProvider.Decrypt(encrypted2);
            Assert.Equal("test", decrypted1);
            Assert.Equal("test", decrypted2);
        }
    }
}
