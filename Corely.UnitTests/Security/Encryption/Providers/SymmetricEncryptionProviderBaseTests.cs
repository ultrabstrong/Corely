using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public sealed class SymmetricEncryptionProviderBaseTests : SymmetricEncryptionProviderGenericTests
    {
        private class MockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
            protected override string DecryptInternal(string value, string key) => value[36..];
        }

        private class NullMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => null!;
            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class EmptyMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => string.Empty;
            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class WhitespaceMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => " ";
            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class ColonMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => "as:df";
            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockEncryptionProvider _mockEncryptionProvider = new();

        [Fact]
        public void NullEncryptionTypeCode_ThrowsArgumentNullException_OnBuild()
        {
            var ex = Record.Exception(() => new NullMockEncryptionProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void EmptyEncryptionTypeCode_ThrowsArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new EmptyMockEncryptionProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void WhitespaceEncryptionTypeCode_ThrowsArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new WhitespaceMockEncryptionProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ColonEncryptionTypeCode_ThrowsArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new ColonMockEncryptionProvider());
            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
        }

        [Fact]
        public override void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(TEST_ENCRYPTION_TYPE_CODE, _mockEncryptionProvider.EncryptionTypeCode);
        }

        public override ISymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new MockEncryptionProvider();
        }

        public override void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            // Don't need to test if MockEncryptionProvider returns the correct key provider
        }
    }
}
