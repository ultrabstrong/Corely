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
        private readonly Fixture _fixture = new();

        [Fact]
        public void AddProvider_ShouldAddProvider()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IEncryptionProvider>().Object;

            _encryptionProviderFactory.AddProvider(providerCode, provider);
            var encryptionProvider = _encryptionProviderFactory.GetProvider(providerCode);

            Assert.NotNull(encryptionProvider);
        }

        [Fact]
        public void AddProvider_ShouldThrowEncryptionProviderException_WithExistingProviderCode()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IEncryptionProvider>().Object;

            _encryptionProviderFactory.AddProvider(providerCode, provider);
            void act() => _encryptionProviderFactory.AddProvider(providerCode, provider);
            var exception = Record.Exception(() => act());

            Assert.NotNull(exception);
            Assert.IsType<EncryptionProviderException>(exception);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void AddProvider_ShouldThrow_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IEncryptionProvider>().Object;

            void act() => _encryptionProviderFactory.AddProvider(providerCode, provider);
            var exception = Record.Exception(() => act());

            Assert.NotNull(exception);
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is EncryptionProviderException);
        }

        [Fact]
        public void AddProvider_ShouldThrowNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            void act() => _encryptionProviderFactory.AddProvider(providerCode, null);
            var exception = Record.Exception(() => act());

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void UpdateProvider_ShouldUpdateProvider()
        {
            var providerCode = _fixture.Create<string>();
            var originalProvider = new Mock<IEncryptionProvider>().Object;
            var updatedProvider = new Mock<IEncryptionProvider>().Object;

            _encryptionProviderFactory.AddProvider(providerCode, originalProvider);
            _encryptionProviderFactory.UpdateProvider(providerCode, updatedProvider);
            var encryptionProvider = _encryptionProviderFactory.GetProvider(providerCode);

            Assert.Equal(updatedProvider, encryptionProvider);
        }

        [Fact]
        public void UpdateProvider_ShouldThrowEncryptionProviderException_WithNonExistingProviderCode()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IEncryptionProvider>().Object;

            void act() => _encryptionProviderFactory.UpdateProvider(providerCode, provider);
            var exception = Record.Exception(() => act());

            Assert.NotNull(exception);
            Assert.IsType<EncryptionProviderException>(exception);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void UpdateProvider_ShouldThrow_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IEncryptionProvider>().Object;

            void act() => _encryptionProviderFactory.UpdateProvider(providerCode, provider);
            var exception = Record.Exception(() => act());

            Assert.NotNull(exception);
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is EncryptionProviderException);
        }

        [Fact]
        public void UpdateProvider_ShouldThrowNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            void act() => _encryptionProviderFactory.UpdateProvider(providerCode, null);
            var exception = Record.Exception(() => act());

            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData(EncryptionProviderConstants.AES_CODE, typeof(AesEncryptionProvider))]
        public void GetProvider_ShouldReturnEncryptionProvider(string code, Type expectedType)
        {
            var encryptionProvider = _encryptionProviderFactory.GetProvider(code);
            Assert.NotNull(encryptionProvider);
            Assert.IsType(expectedType, encryptionProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void GetProvider_ShouldThrow_WithInvalidCode(string code)
        {
            void act() => _encryptionProviderFactory.GetProvider(code);
            var exception = Record.Exception(() => act());
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is EncryptionProviderException);
        }

        [Theory]
        [InlineData(EncryptionProviderConstants.AES_CODE, typeof(AesEncryptionProvider))]
        public void GetProviderForDecrypting_ShouldReturnEncryptionProvider(string code, Type expectedType)
        {
            var fixture = new Fixture();
            var encryptedValue = $"{code}:1:{fixture.Create<string>()}";
            var encryptionProvider = _encryptionProviderFactory.GetProviderForDecrypting(encryptedValue);
            Assert.NotNull(encryptionProvider);
            Assert.IsType(expectedType, encryptionProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void GetProviderForDecrypting_ShouldThrow_WithInvalidCode(string code)
        {
            void act() => _encryptionProviderFactory.GetProviderForDecrypting(code);
            var exception = Record.Exception(() => act());
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is EncryptionProviderException);
        }

        [Fact]
        public void ListProviders_ShouldReturnListOfProviders()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IEncryptionProvider>().Object;

            var providers = _encryptionProviderFactory.ListProviders();
            _encryptionProviderFactory.AddProvider(providerCode, provider);
            var updatedProviders = _encryptionProviderFactory.ListProviders();

            Assert.NotNull(providers);
            Assert.NotEmpty(providers);
            Assert.NotNull(updatedProviders);
            Assert.NotEmpty(updatedProviders);
            Assert.True(providers.Count < updatedProviders.Count);
        }
    }
}
