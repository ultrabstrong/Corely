using AutoFixture;
using Corely.Shared.Providers.Security;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Security
{
    public class EncryptionProviderBaseTests
    {
        public class MockEncryptionProvider : EncryptionProviderBase
        {
            protected override string TwoDigitEncryptionTypeCode => TwoDigitEncryptionTypeCodeForTest;

            public string TwoDigitEncryptionTypeCodeForTest { get; set; } = EncryptionProviderCodeConstants.AES;

            public MockEncryptionProvider(ISecretProvider secretProvider)
                : base(secretProvider)
            {
            }

            protected override string DecryptInternal(string value)
            {
                return value;
            }

            protected override string EncryptInternal(string value)
            {
                return value;
            }
        }

        private readonly MockEncryptionProvider _mockEncryptionProvider = new(new Mock<ISecretProvider>().Object);
        private readonly Fixture _fixture = new();

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Decrypt_ShouldThrow_WithNullOrEmpty(string value)
        {
            var exception = Record.Exception(() => _mockEncryptionProvider.Decrypt(value));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Encrypt_ShouldThrow_WithNullOrEmpty(string value)
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
    }
}
