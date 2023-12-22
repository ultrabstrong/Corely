using Corely.Shared.Providers.Security.Encryption;
using Corely.Shared.Providers.Security.Exceptions;
using Corely.Shared.Providers.Security.Factories;
using Corely.Shared.Providers.Security.Secrets;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Security.Factories
{
    public class EncryptionProviderFactoryTests
    {
        private readonly EncryptionProviderFactory _encryptionProviderFactory =
            new(new Mock<ISecretProvider>().Object);

        [Fact]
        public void Create_ShouldReturnEncryptionProvider()
        {
            var encryptionProvider = _encryptionProviderFactory.Create();
            Assert.NotNull(encryptionProvider);
            Assert.Equal(typeof(AesEncryptionProvider), encryptionProvider.GetType());
        }

        [Theory]
        [InlineData(EncryptionProviderConstants.Aes, typeof(AesEncryptionProvider))]
        public void CreateForDecrypting_ShouldReturnEncryptionProvider(string code, Type expectedType)
        {
            var encryptionProvider = _encryptionProviderFactory.CreateForDecrypting(code);
            Assert.NotNull(encryptionProvider);
            Assert.Equal(expectedType, encryptionProvider.GetType());
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("A")]
        [InlineData("1")]
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
