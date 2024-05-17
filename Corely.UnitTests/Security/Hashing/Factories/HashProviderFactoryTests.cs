using AutoFixture;
using Corely.Security.Hashing;
using Corely.Security.Hashing.Factories;
using Corely.Security.Hashing.Providers;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Security.Hashing.Factories
{
    public class HashProviderFactoryTests
    {
        private readonly string _defaultProviderCode = HashConstants.SALTED_SHA256_CODE;
        private readonly Fixture _fixture = new();
        private readonly HashProviderFactory _hashProviderFactory;

        public HashProviderFactoryTests()
        {
            _hashProviderFactory = new HashProviderFactory(_defaultProviderCode);
        }

        [Fact]
        public void AddProvider_AddsProvider()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IHashProvider>().Object;

            _hashProviderFactory.AddProvider(providerCode, provider);
            var hashProvider = _hashProviderFactory.GetProvider(providerCode);

            Assert.NotNull(hashProvider);
        }

        [Fact]
        public void AddProvider_ThrowsHashProviderException_WithExistingProviderCode()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IHashProvider>().Object;

            _hashProviderFactory.AddProvider(providerCode, provider);
            void act() => _hashProviderFactory.AddProvider(providerCode, provider);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<HashException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void AddProvider_Throws_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IHashProvider>().Object;

            void act() => _hashProviderFactory.AddProvider(providerCode, provider);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is HashException);
        }

        [Fact]
        public void AddProvider_ThrowsNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            void act() => _hashProviderFactory.AddProvider(providerCode, null);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void UpdateProvider_UpdatesProvider()
        {
            var providerCode = _fixture.Create<string>();
            var originalProvider = new Mock<IHashProvider>().Object;
            var updatedProvider = new Mock<IHashProvider>().Object;

            _hashProviderFactory.AddProvider(providerCode, originalProvider);
            _hashProviderFactory.UpdateProvider(providerCode, updatedProvider);
            var hashProvider = _hashProviderFactory.GetProvider(providerCode);

            Assert.Equal(updatedProvider, hashProvider);
        }

        [Fact]
        public void UpdateProvider_ThrowsHashProviderException_WithNonExistingProviderCode()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IHashProvider>().Object;

            void act() => _hashProviderFactory.UpdateProvider(providerCode, provider);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<HashException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void UpdateProvider_Throws_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IHashProvider>().Object;

            void act() => _hashProviderFactory.UpdateProvider(providerCode, provider);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is HashException);
        }

        [Fact]
        public void UpdateProvider_ThrowsNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            void act() => _hashProviderFactory.UpdateProvider(providerCode, null);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void GetDefaultProvider_ReturnsDefaultProvider()
        {
            var hashProvider = _hashProviderFactory.GetDefaultProvider();
            Assert.NotNull(hashProvider);
            Assert.Equal(_defaultProviderCode, hashProvider.HashTypeCode);
        }

        [Theory]
        [InlineData(HashConstants.SALTED_SHA256_CODE, typeof(Sha256SaltedHashProvider))]
        [InlineData(HashConstants.SALTED_SHA512_CODE, typeof(Sha512SaltedHashProvider))]
        public void GetProvider_ReturnProvider(string providerCode, Type expectedType)
        {
            var hashProvider = _hashProviderFactory.GetProvider(providerCode);
            Assert.NotNull(hashProvider);
            Assert.IsType(expectedType, hashProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void GetProvider_Throws_WithInvalidCode(string providerCode)
        {
            void act() => _hashProviderFactory.GetProvider(providerCode);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is HashException);
        }

        [Theory]
        [InlineData(HashConstants.SALTED_SHA256_CODE, typeof(Sha256SaltedHashProvider))]
        [InlineData(HashConstants.SALTED_SHA512_CODE, typeof(Sha512SaltedHashProvider))]
        public void GetProviderToVerify_ReturnHashProvider(string providerCode, Type expectedType)
        {
            var fixture = new Fixture();
            var hashedValue = $"{providerCode}:{fixture.Create<string>()}";
            var hashProvider = _hashProviderFactory.GetProviderToVerify(hashedValue);
            Assert.NotNull(hashProvider);
            Assert.IsType(expectedType, hashProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void GetProviderToVerify_Throws_WithInvalidCode(string providerCode)
        {
            void act() => _hashProviderFactory.GetProviderToVerify(providerCode);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is HashException);
        }

        [Fact]
        public void ListProviders_ReturnsListOfProviders()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IHashProvider>().Object;

            var providers = _hashProviderFactory.ListProviders();
            _hashProviderFactory.AddProvider(providerCode, provider);
            var updatedProviders = _hashProviderFactory.ListProviders();

            Assert.NotNull(providers);
            Assert.NotEmpty(providers);
            Assert.NotNull(updatedProviders);
            Assert.NotEmpty(updatedProviders);
            Assert.True(providers.Count < updatedProviders.Count);
        }
    }
}
