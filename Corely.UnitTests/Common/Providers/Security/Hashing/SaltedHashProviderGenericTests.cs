using AutoFixture;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.UnitTests.Common.Providers.Security.Hashing
{
    public abstract class SaltedHashProviderGenericTests
    {
        protected abstract IHashProvider HashProvider { get; }

        private readonly Fixture _fixture = new();

        [Fact]
        public void Hash_ShouldReturnHashedValue()
        {
            var value = _fixture.Create<string>();
            var hashed = HashProvider.Hash(value);
            Assert.NotEqual(value, hashed);
        }

        [Fact]
        public void Hash_ShouldProduceDifferentValues_WithSameInput()
        {
            var value = _fixture.Create<string>();
            var hash1 = HashProvider.Hash(value);
            var hash2 = HashProvider.Hash(value);
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void Hash_ShouldReturnHashedString_WithHashTypePrepended()
        {
            var value = _fixture.Create<string>();
            var hashed = HashProvider.Hash(value);
            Assert.StartsWith(HashProvider.HashTypeCode, hashed);
            Assert.NotEqual(value, hashed[hashed.IndexOf(':')..]);
        }

        [Fact]
        public void Hash_ShouldThrowArgumentNullException_WhenValueIsNull()
        {
            void act() => HashProvider.Hash(null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void Verify_ShouldReturnTrue_WithMatchingValueHash()
        {
            var value = _fixture.Create<string>();
            var hash = HashProvider.Hash(value);
            var isVerified = HashProvider.Verify(value, hash);
            Assert.True(isVerified);
        }

        [Fact]
        public void Verify_ShouldReturnFalse_WithNonMatchingHash()
        {
            var value = _fixture.Create<string>();
            var hash = HashProvider.Hash(value);
            var isVerified = HashProvider.Verify(value + "1", hash);
            Assert.False(isVerified);
        }

        [Fact]
        public void Verify_ShouldReturnFalse_WithNonMatchingHashType()
        {
            var value = _fixture.Create<string>();
            var hash = HashProvider.Hash(value);
            var isVerified = HashProvider.Verify(value, hash.Replace(HashProvider.HashTypeCode, "--"));
            Assert.False(isVerified);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        public void Verify_ShouldThrowArgumentNullException_WithNull(string? value, string? hash)
        {
            void act() => HashProvider.Verify(value, hash);
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public abstract void HashTypeCode_ShouldReturnCorrectCode_ForImplementation();
    }
}
