using AutoFixture;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Hashing;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Providers.Security.Factories
{
    public class HashProviderFactoryTests
    {
        private readonly HashProviderFactory _hashProviderFactory = new();

        [Theory]
        [InlineData(HashProviderConstants.SALTED_SHA256, typeof(Sha256SaltedHashProvider))]
        [InlineData(HashProviderConstants.SALTED_SHA512, typeof(Sha512SaltedHashProvider))]
        public void Create(string providerCode, Type expectedType)
        {
            var hashProvider = _hashProviderFactory.Create(providerCode);
            Assert.NotNull(hashProvider);
            Assert.IsType(expectedType, hashProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void Create_ShouldThrow_WithInvalidCode(string providerCode)
        {
            void act() => _hashProviderFactory.Create(providerCode);
            var exception = Record.Exception(() => act());
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is HashProviderException);
        }

        [Theory]
        [InlineData(HashProviderConstants.SALTED_SHA256, typeof(Sha256SaltedHashProvider))]
        [InlineData(HashProviderConstants.SALTED_SHA512, typeof(Sha512SaltedHashProvider))]
        public void CreateToVerify_ShouldReturnHashProvider(string providerCode, Type expectedType)
        {
            var fixture = new Fixture();
            var hashedValue = $"{providerCode}:{fixture.Create<string>()}";
            var hashProvider = _hashProviderFactory.CreateToVerify(hashedValue);
            Assert.NotNull(hashProvider);
            Assert.IsType(expectedType, hashProvider);
        }

        [Theory]
        [ClassData(typeof(NullEmptyAndWhitespace))]
        [InlineData("-")]
        [InlineData("--")]
        public void CreateToVerify_ShouldThrow_WithInvalidCode(string providerCode)
        {
            void act() => _hashProviderFactory.CreateToVerify(providerCode);
            var exception = Record.Exception(() => act());
            Assert.True(exception is ArgumentNullException
                || exception is ArgumentException
                || exception is HashProviderException);
        }
    }
}
