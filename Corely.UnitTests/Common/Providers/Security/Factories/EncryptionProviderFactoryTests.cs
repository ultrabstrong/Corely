using AutoFixture;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Keys;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Providers.Security.Factories
{
    public class EncryptionProviderFactoryTests
    {
        private readonly EncryptionProviderFactory _encryptionProviderFactory =
            new(new Mock<IKeyStoreProvider>().Object);

        [Theory]
        [InlineData(EncryptionProviderConstants.AES, typeof(AesEncryptionProvider))]
        public void Create_ShouldReturnEncryptionProvider(string code, Type expectedType)
        {
            var encryptionProvider = _encryptionProviderFactory.Create(code);
            Assert.NotNull(encryptionProvider);
            Assert.IsType(expectedType, encryptionProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void Create_ShouldThrow_WithInvalidCode(string code)
        {
            void act() => _encryptionProviderFactory.Create(code);
            var exception = Record.Exception(() => act());
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is EncryptionProviderException);
        }

        [Theory]
        [InlineData(EncryptionProviderConstants.AES, typeof(AesEncryptionProvider))]
        public void CreateForDecrypting_ShouldReturnEncryptionProvider(string code, Type expectedType)
        {
            var fixture = new Fixture();
            var encryptedValue = $"{code}:1:{fixture.Create<string>()}";
            var encryptionProvider = _encryptionProviderFactory.CreateForDecrypting(encryptedValue);
            Assert.NotNull(encryptionProvider);
            Assert.IsType(expectedType, encryptionProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void CreateForDecrypting_ShouldThrow_WithInvalidCode(string code)
        {
            void act() => _encryptionProviderFactory.CreateForDecrypting(code);
            var exception = Record.Exception(() => act());
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is EncryptionProviderException);
        }
    }
}
