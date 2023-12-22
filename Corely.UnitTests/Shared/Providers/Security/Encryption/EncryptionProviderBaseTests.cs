using AutoFixture;
using Corely.Shared.Providers.Security.Encryption;
using Corely.Shared.Providers.Security.Exceptions;
using Corely.Shared.Providers.Security.Secrets;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Security.Encryption
{
    public class EncryptionProviderBaseTests
    {
        private class MockEncryptionProvider : EncryptionProviderBase
        {
            protected override string TwoDigitEncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            public string EncryptionTypeCode => TwoDigitEncryptionTypeCode;
            public MockEncryptionProvider(ISecretProvider secretProvider)
                : base(secretProvider) { }

            protected override string DecryptInternal(string value, string key) => value;
            protected override string EncryptInternal(string value, string key) => value;
        }

        private class InvalidMockEncryptionProvider : EncryptionProviderBase
        {
            protected override string TwoDigitEncryptionTypeCode => "asdf";

            public InvalidMockEncryptionProvider(ISecretProvider secretProvider)
                : base(secretProvider) { }

            protected override string DecryptInternal(string value, string key) => value;
            protected override string EncryptInternal(string value, string key) => value;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockEncryptionProvider _mockEncryptionProvider = new(new Mock<ISecretProvider>().Object);
        private readonly Fixture _fixture = new();

        [Fact]
        public void InvalidEncryptionTypeCode_ShouldThrowOnBuild()
        {
            void act() => new InvalidMockEncryptionProvider(new Mock<ISecretProvider>().Object);
            Assert.Throws<EncryptionProviderException>(act);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Decrypt_ShouldThrow_WithNullOrWhiteSpace(string value)
        {
            var exception = Record.Exception(() => _mockEncryptionProvider.Decrypt(value));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Encrypt_ShouldThrow_WithNullOrWhiteSpace(string value)
        {
            var exception = Record.Exception(() => _mockEncryptionProvider.Encrypt(value));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Fact]
        public void Decrypt_ShouldReturnSameString_WithEncryptionTypeStripped()
        {
            var testValue = _fixture.Create<string>();
            var encrypted = _mockEncryptionProvider.Encrypt(testValue);
            var decrypted = _mockEncryptionProvider.Decrypt(encrypted);
            Assert.Equal(testValue, decrypted);
        }

        [Fact]
        public void Encrypt_ShouldReturnCorrectlyFormattedValue()
        {
            var testValue = _fixture.Create<string>();

            var encrypted = _mockEncryptionProvider.Encrypt(testValue);

            Assert.StartsWith(_mockEncryptionProvider.EncryptionTypeCode, encrypted);
            Assert.Matches(@"^\d{2}\d+:\w+", encrypted);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        [InlineData("a1")]
        [InlineData("1a")]
        [InlineData("aa")]
        public void Decrypt_ShouldThrow_WithInvalidEncryptionTypeCode(string encryptionCode)
        {
            var version = _fixture.Create<int>();
            var encryptedValue = $"{encryptionCode}{version}:{_fixture.Create<string>()}";
            void act() => _mockEncryptionProvider.Decrypt(encryptedValue);
            Assert.Throws<EncryptionProviderException>(act);
        }

        [Theory]
        [InlineData(TEST_ENCRYPTION_TYPE_CODE)]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}1")]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}a")]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}1:")]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}a:")]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}:")]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}:1")]
        [InlineData($"{TEST_ENCRYPTION_TYPE_CODE}:a")]
        [InlineData($"1:1")]
        [InlineData($"a:1")]
        [InlineData($"1:a")]
        [InlineData($"1:")]
        [InlineData($"a:")]
        [InlineData($":1")]
        [InlineData($":a")]
        public void Decrypt_ShouldThrow_WithInvalidFormat(string value)
        {
            void act() => _mockEncryptionProvider.Decrypt(value);
            Assert.Throws<EncryptionProviderException>(act);
        }

        [Fact]
        public void ReEncryptWithCurrentKey_ShouldReturnCurrentValue_IfVersionHasntChanged()
        {
            var secretProvider = new InMemorySecretProvider(_fixture.Create<string>());
            var encryptionProvider = new MockEncryptionProvider(secretProvider);

            var originalValue = _fixture.Create<string>();
            var originalEncrypted = encryptionProvider.Encrypt(originalValue);

            var reEncrypted = encryptionProvider.ReEncryptWithCurrentKey(originalEncrypted);
            var decrypted = encryptionProvider.Decrypt(reEncrypted);

            Assert.Equal(originalValue, decrypted);
            Assert.Equal(originalEncrypted, reEncrypted);
            Assert.Equal($"{TEST_ENCRYPTION_TYPE_CODE}1:{originalValue}", originalEncrypted);
        }

        [Fact]
        public void ReEncryptWithCurrentKey_ShouldReturnNewValue_IfVersionHasChanged()
        {
            var secretProvider = new InMemorySecretProvider(_fixture.Create<string>());
            var encryptionProvider = new MockEncryptionProvider(secretProvider);

            var originalValue = _fixture.Create<string>();
            var originalEncrypted = encryptionProvider.Encrypt(originalValue);

            secretProvider.Add(_fixture.Create<string>());

            var reEncrypted = encryptionProvider.ReEncryptWithCurrentKey(originalEncrypted);
            var decrypted = encryptionProvider.Decrypt(reEncrypted);

            Assert.Equal(originalValue, decrypted);
            Assert.NotEqual(originalEncrypted, reEncrypted);
            Assert.Equal($"{TEST_ENCRYPTION_TYPE_CODE}1:{originalValue}", originalEncrypted);
            Assert.Equal($"{TEST_ENCRYPTION_TYPE_CODE}2:{originalValue}", reEncrypted);
        }
    }
}
