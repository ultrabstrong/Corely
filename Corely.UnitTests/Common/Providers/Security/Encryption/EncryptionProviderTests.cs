using AutoFixture;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Keys;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Providers.Security.Encryption
{
    public abstract class EncryptionProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly AesKeyProvider _keyProvider;
        private readonly InMemoryKeyStoreProvider _keyStoreProvider;
        private readonly IEncryptionProvider _encryptionProvider;

        public EncryptionProviderTests()
        {
            _keyProvider = new AesKeyProvider();
            _keyStoreProvider = new InMemoryKeyStoreProvider(_keyProvider.CreateKey());
            _encryptionProvider = GetEncryptionProvider(_keyStoreProvider);
        }

        [Fact]
        public void Encrypt_ShouldReturnCorrectlyFormattedValue()
        {
            var decrypted = _fixture.Create<string>();

            var encrypted = _encryptionProvider.Encrypt(decrypted);

            Assert.StartsWith(_encryptionProvider.EncryptionTypeCode, encrypted);
            Assert.Matches(@"^.+:\d+:\w+", encrypted);
            Assert.NotEqual(decrypted, encrypted);
        }

        [Fact]
        public void Encrypt_ShouldProduceDifferentEncryptedStrings()
        {
            var decrypted = _fixture.Create<string>();
            var encrypted1 = _encryptionProvider.Encrypt(decrypted);
            var encrypted2 = _encryptionProvider.Encrypt(decrypted);
            Assert.NotEqual(encrypted1, encrypted2);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Encrypt_ShouldArgumentExceptionThrow_WithNullOrWhiteSpace(string value)
        {
            var exception = Record.Exception(() => _encryptionProvider.Encrypt(value));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Fact]
        public void Encrypt_ThenDecrypt_ShouldProduceOriginalValue()
        {
            var originalDecrypted = _fixture.Create<string>();
            var encrypted = _encryptionProvider.Encrypt(originalDecrypted);
            var decrypted = _encryptionProvider.Decrypt(encrypted);
            Assert.Equal(originalDecrypted, decrypted);
        }

        [Fact]
        public void Decrypt_ShouldProduceSameStringThatWasEncrypted()
        {
            var decrpyted = _fixture.Create<string>();
            var encrypted1 = _encryptionProvider.Encrypt(decrpyted);
            var encrypted2 = _encryptionProvider.Encrypt(decrpyted);
            var decrypted1 = _encryptionProvider.Decrypt(encrypted1);
            var decrypted2 = _encryptionProvider.Decrypt(encrypted2);
            Assert.Equal(decrpyted, decrypted1);
            Assert.Equal(decrpyted, decrypted2);
        }

        [Fact]
        public void Decrypt_ShouldSucceed_AfterKeyIsUpdated()
        {
            var decrypted = _fixture.Create<string>();
            var encrypted = _encryptionProvider.Encrypt(decrypted);

            _keyStoreProvider.Add(_keyProvider.CreateKey());

            var reDecrypted = _encryptionProvider.Decrypt(encrypted);
            Assert.Equal(decrypted, reDecrypted);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Decrypt_ShouldThrowArgumentException_WithNullOrWhiteSpace(string value)
        {
            var exception = Record.Exception(() => _encryptionProvider.Decrypt(value));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Theory]
        [InlineData($"--", true)]
        [InlineData($"--:", false)]
        [InlineData($"--::", false)]
        [InlineData($":", false)]
        [InlineData($"::", false)]
        [InlineData($"", true)]
        [InlineData($":", true)]
        [InlineData($"::", true)]
        [InlineData($":1", true)]
        [InlineData($":2:", true)]
        public void Decrypt_ShouldThrowEncryptionProviderException_WithInvalidFormat(string value, bool prependTypeCode)
        {
            var testValue = prependTypeCode
                ? $"{_encryptionProvider.EncryptionTypeCode}{value}"
                : value;
            void act() => _encryptionProvider.Decrypt(testValue);
            Assert.Throws<EncryptionProviderException>(act);
        }

        [Fact]
        public void ReEncrypt_ShouldReturnUpdatedCurrentVersionValue_WhenKeyVersionIsSame()
        {
            var originalValue = _fixture.Create<string>();
            var originalEncrypted = _encryptionProvider.Encrypt(originalValue);

            var reEncrypted = _encryptionProvider.ReEncrypt(originalEncrypted);
            var decrypted = _encryptionProvider.Decrypt(reEncrypted);

            Assert.Equal(originalValue, decrypted);
            Assert.NotEqual(originalEncrypted, reEncrypted);
            Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:1:", originalEncrypted);
            Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:1:", reEncrypted);
        }

        [Fact]
        public void ReEncrypt_ShouldReturnUpdatedNewVersionValue_WhenKeyVersionIsChanged()
        {
            var originalValue = _fixture.Create<string>();
            var originalEncrypted = _encryptionProvider.Encrypt(originalValue);

            _keyStoreProvider.Add(_keyProvider.CreateKey());

            var reEncrypted = _encryptionProvider.ReEncrypt(originalEncrypted);
            var decrypted = _encryptionProvider.Decrypt(reEncrypted);

            Assert.Equal(originalValue, decrypted);
            Assert.NotEqual(originalEncrypted, reEncrypted);
            Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:1:", originalEncrypted);
            Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:2:", reEncrypted);
        }

        [Fact]
        public abstract void EncryptionTypeCode_ShouldReturnCorrectCode_ForImplementation();

        public abstract IEncryptionProvider GetEncryptionProvider(IKeyStoreProvider keyStoreProvider);

    }
}
