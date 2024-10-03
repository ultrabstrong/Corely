using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public sealed class AsymmetricEncryptionProviderBaseTests : AsymmetricEncryptionProviderGenericTests
    {
        private class MockEncryptionProvider : AsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
            protected override string DecryptInternal(string value, string key) => value[36..];
        }

        private class NullMockEncryptionProvider : AsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => null!;
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class EmptyMockEncryptionProvider : AsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => string.Empty;
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class WhitespaceMockEncryptionProvider : AsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => " ";
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class ColonMockEncryptionProvider : AsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => "as:df";
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
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

        public override IAsymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new MockEncryptionProvider();
        }

        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            // Don't need to test if MockEncryptionProvider returns the correct key provider
        }
    }
}
