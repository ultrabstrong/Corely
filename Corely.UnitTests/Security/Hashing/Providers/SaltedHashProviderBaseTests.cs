using AutoFixture;
using Corely.Security.Hashing;
using Corely.Security.Hashing.Providers;

namespace Corely.UnitTests.Security.Hashing.Providers
{
    public class SaltedHashProviderBaseTests : SaltedHashProviderGenericTests
    {
        private class MockHashProvider : SaltedHashProviderBase
        {
            public override string HashTypeCode => TEST_HASH_TYPE_CODE;
            protected override byte[] HashInternal(byte[] value) => value;
        }

        private class NullTypeCodeMockHashProvider : SaltedHashProviderBase
        {
            public override string HashTypeCode => null!;
            protected override byte[] HashInternal(byte[] value) => value;
        }

        private class EmptyTypeCodeMockHashProvider : SaltedHashProviderBase
        {
            public override string HashTypeCode => "";
            protected override byte[] HashInternal(byte[] value) => value;
        }

        private class WhitespaceTypeCodeMockHashProvider : SaltedHashProviderBase
        {
            public override string HashTypeCode => " ";
            protected override byte[] HashInternal(byte[] value) => value;
        }

        private class ColonTypeCodeMockHashProvider : SaltedHashProviderBase
        {
            public override string HashTypeCode => "as:df";
            protected override byte[] HashInternal(byte[] value) => value;
        }

        protected override IHashProvider HashProvider => _mockHashProvider;

        private const string TEST_HASH_TYPE_CODE = "00";

        private readonly MockHashProvider _mockHashProvider = new();

        [Fact]
        public void NullHashTypeCode_ShouldThrowArgumentNullException_OnBuild()
        {
            var ex = Record.Exception(() => new NullTypeCodeMockHashProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void EmptyHashTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new EmptyTypeCodeMockHashProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void WhitespaceHashTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new WhitespaceTypeCodeMockHashProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ColonHashTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new ColonTypeCodeMockHashProvider());
            Assert.NotNull(ex);
            Assert.IsType<HashException>(ex);
        }

        [Fact]
        public override void HashTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(TEST_HASH_TYPE_CODE, _mockHashProvider.HashTypeCode);
        }

        [Theory]
        [InlineData("asdf")]
        [InlineData(TEST_HASH_TYPE_CODE)]
        [InlineData($"{TEST_HASH_TYPE_CODE}:asdf")]
        public void Verify_ShouldReturnFalse_WithInvalidHash(string hash)
        {
            var fixture = new Fixture();
            Assert.False(_mockHashProvider.Verify(fixture.Create<string>(), hash));
        }
    }
}
