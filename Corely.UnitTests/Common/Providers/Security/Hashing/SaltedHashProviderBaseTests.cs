using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.UnitTests.Common.Providers.Security.Hashing
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
#pragma warning disable CS8603 // Possible null reference return.
            public override string HashTypeCode => null;
#pragma warning restore CS8603 // Possible null reference return.
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
            static void act() => new NullTypeCodeMockHashProvider();
            Assert.Throws<ArgumentNullException>(act);
        }

        [Fact]
        public void EmptyHashTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            static void act() => new EmptyTypeCodeMockHashProvider();
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void WhitespaceHashTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            static void act() => new WhitespaceTypeCodeMockHashProvider();
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ColonHashTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            static void act() => new ColonTypeCodeMockHashProvider();
            Assert.Throws<HashProviderException>(act);
        }

        [Fact]
        public override void HashTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(TEST_HASH_TYPE_CODE, _mockHashProvider.HashTypeCode);
        }
    }
}
