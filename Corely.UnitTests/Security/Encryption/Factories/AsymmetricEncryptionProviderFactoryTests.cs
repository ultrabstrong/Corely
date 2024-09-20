﻿using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Providers;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Security.Encryption.Factories
{
    public class AsymmetricEncryptionProviderFactoryTests
    {
        private readonly string _defaultProviderCode = AsymmetricEncryptionConstants.RSA_SHA256_CODE;
        private readonly AsymmetricEncryptionProviderFactory _encryptionProviderFactory;
        private readonly Fixture _fixture = new();

        public AsymmetricEncryptionProviderFactoryTests()
        {
            _encryptionProviderFactory = new(_defaultProviderCode);
        }

        [Fact]
        public void AddProvider_AddProvider()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IAsymmetricEncryptionProvider>().Object;

            _encryptionProviderFactory.AddProvider(providerCode, provider);
            var encryptionProvider = _encryptionProviderFactory.GetProvider(providerCode);

            Assert.NotNull(encryptionProvider);
        }

        [Fact]
        public void AddProvider_ThrowsEncryptionProviderException_WithExistingProviderCode()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IAsymmetricEncryptionProvider>().Object;

            _encryptionProviderFactory.AddProvider(providerCode, provider);
            var ex = Record.Exception(() => _encryptionProviderFactory.AddProvider(providerCode, provider));

            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void AddProvider_Throws_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IAsymmetricEncryptionProvider>().Object;

            var ex = Record.Exception(() => _encryptionProviderFactory.AddProvider(providerCode, provider));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Fact]
        public void AddProvider_ThrowsArgumentNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            var ex = Record.Exception(() => _encryptionProviderFactory.AddProvider(providerCode, null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void UpdateProvider_UpdateProvider()
        {
            var providerCode = _fixture.Create<string>();
            var originalProvider = new Mock<IAsymmetricEncryptionProvider>().Object;
            var updatedProvider = new Mock<IAsymmetricEncryptionProvider>().Object;

            _encryptionProviderFactory.AddProvider(providerCode, originalProvider);
            _encryptionProviderFactory.UpdateProvider(providerCode, updatedProvider);
            var encryptionProvider = _encryptionProviderFactory.GetProvider(providerCode);

            Assert.Equal(updatedProvider, encryptionProvider);
        }

        [Fact]
        public void UpdateProvider_ThrowsEncryptionProviderException_WithNonExistingProviderCode()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IAsymmetricEncryptionProvider>().Object;

            var ex = Record.Exception(() => _encryptionProviderFactory.UpdateProvider(providerCode, provider));

            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData(":")]
        public void UpdateProvider_Throws_WithInvalidCode(string providerCode)
        {
            var provider = new Mock<IAsymmetricEncryptionProvider>().Object;

            var ex = Record.Exception(() => _encryptionProviderFactory.UpdateProvider(providerCode, provider));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Fact]
        public void UpdateProvider_ThrowsArgumentNullException_WithNullProvider()
        {
            var providerCode = _fixture.Create<string>();

            var ex = Record.Exception(() => _encryptionProviderFactory.UpdateProvider(providerCode, null));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void GetDefaultProvider_ReturnsDefaultProvider()
        {
            var encryptionProvider = _encryptionProviderFactory.GetDefaultProvider();

            Assert.NotNull(encryptionProvider);
            Assert.Equal(_defaultProviderCode, encryptionProvider.EncryptionTypeCode);
        }

        [Theory]
        [InlineData(AsymmetricEncryptionConstants.RSA_SHA256_CODE, typeof(RsaSha256EncryptionProvider))]
        public void GetProvider_ReturnEncryptionProvider(string code, Type expectedType)
        {
            var encryptionProvider = _encryptionProviderFactory.GetProvider(code);
            Assert.NotNull(encryptionProvider);
            Assert.IsType(expectedType, encryptionProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void GetProvider_Throws_WithInvalidCode(string code)
        {
            var ex = Record.Exception(() => _encryptionProviderFactory.GetProvider(code));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Theory]
        [InlineData(AsymmetricEncryptionConstants.RSA_SHA256_CODE, typeof(RsaSha256EncryptionProvider))]
        public void GetProviderForDecrypting_ReturnsEncryptionProvider(string code, Type expectedType)
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
        public void GetProviderForDecrypting_Throws_WithInvalidCode(string code)
        {
            var ex = Record.Exception(() => _encryptionProviderFactory.GetProviderForDecrypting(code));

            Assert.NotNull(ex);
            Assert.True(ex is ArgumentNullException
                || ex is ArgumentException
                || ex is EncryptionException);
        }

        [Fact]
        public void ListProviders_ReturnsListOfProviders()
        {
            var providerCode = _fixture.Create<string>();
            var provider = new Mock<IAsymmetricEncryptionProvider>().Object;

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