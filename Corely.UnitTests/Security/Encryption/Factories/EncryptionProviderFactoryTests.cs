using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Security.Encryption.Factories
{
    public class EncryptionProviderFactoryTests
    {
        private readonly string _defaultProviderCode = EncryptionConstants.AES_CODE;
        private readonly EncryptionProviderFactory _encryptionProviderFactory;
        private readonly Fixture _fixture = new();

        public EncryptionProviderFactoryTests()
        {
            _encryptionProviderFactory = new(_defaultProviderCode, new Mock<IKeyStoreProvider>().Object);
        }

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
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void AddProvider_ShouldThrow_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IEncryptionProvider>().Object;

            void act() => _encryptionProviderFactory.AddProvider(providerCode, provider);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Fact]
        public void AddProvider_ShouldThrowNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            void act() => _encryptionProviderFactory.AddProvider(providerCode, null);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
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
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void UpdateProvider_ShouldThrow_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IEncryptionProvider>().Object;

            void act() => _encryptionProviderFactory.UpdateProvider(providerCode, provider);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Fact]
        public void UpdateProvider_ShouldThrowNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            void act() => _encryptionProviderFactory.UpdateProvider(providerCode, null);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void GetDefaultProvider_ShouldReturnDefaultProvider()
        {
            var encryptionProvider = _encryptionProviderFactory.GetDefaultProvider();
            Assert.NotNull(encryptionProvider);
            Assert.Equal(_defaultProviderCode, encryptionProvider.EncryptionTypeCode);
        }

        [Theory]
        [InlineData(EncryptionConstants.AES_CODE, typeof(AesEncryptionProvider))]
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
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Theory]
        [InlineData(EncryptionConstants.AES_CODE, typeof(AesEncryptionProvider))]
        public void GetProviderForDecrypting_ShouldReturnEncryptionProvider(string code, Type expectedType)
        {
            var encryptedValue = $"{code}:1:{_fixture.Create<string>()}";
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
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
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
