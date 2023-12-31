using AutoFixture;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.UnitTests.Common.Models.Security
{
    public class HashedValueTests
    {
        private readonly HashedValue _hashedValue;
        private readonly Fixture _fixture = new();

        public HashedValueTests()
        {
            var hashProvider = new Sha256SaltedHashProvider();
            _hashedValue = new HashedValue(hashProvider);
        }

        [Fact]
        public void Constructor_ShouldCreateHashedValue()
        {
            Assert.NotNull(_hashedValue);
        }

        [Fact]
        public void Constructor_ShouldCreateHashedValueWithHash()
        {
            var hashProvider = new Sha256SaltedHashProvider();
            var value = _fixture.Create<string>();

            var hashedValue = new HashedValue(hashProvider) { Hash = value };

            Assert.Equal(value, hashedValue.Hash);
        }

        [Fact]
        public void Set_ShouldSetHashedValue()
        {
            var value = _fixture.Create<string>();
            _hashedValue.Set(value);
            Assert.NotNull(_hashedValue.Hash);
            Assert.NotEmpty(_hashedValue.Hash);
            Assert.NotEqual(value, _hashedValue.Hash);
        }

        [Fact]
        public void Verify_ShouldVerifyHashedValue()
        {
            var value = _fixture.Create<string>();
            _hashedValue.Set(value);
            var verified = _hashedValue.Verify(value);
            Assert.True(verified);
        }
    }
}
