using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public sealed class AsymmetricEncryptionProviderBaseTests : AsymmetricEncryptionProviderGenericTests
    {
        private class MockAsymmetricKeyProvider : IAsymmetricKeyProvider
        {
            private readonly Fixture _fixture = new();

            public (string PublicKey, string PrivateKey) CreateKeys() => _fixture.Create<(string, string)>();

            public bool IsKeyValid(string publicKey, string privateKey) => true;
        }

        private class MockEncryptionProvider : AsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            private readonly MockAsymmetricKeyProvider _mockKeyProvider = new();
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => _mockKeyProvider;
            protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
            protected override string DecryptInternal(string value, string key) => value[36..];
        }

        private abstract class MockAsymmetricEncryptionProviderBase : AsymmetricEncryptionProviderBase
        {
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class NullMockEncryptionProvider : MockAsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => null!;
        }

        private class EmptyMockEncryptionProvider : MockAsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => string.Empty;
        }

        private class WhitespaceMockEncryptionProvider : MockAsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => " ";
        }

        private class ColonMockEncryptionProvider : MockAsymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => "as:df";
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

        [Fact]
        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _mockEncryptionProvider.GetAsymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<MockAsymmetricKeyProvider>(keyProvider);
        }

        public override IAsymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new MockEncryptionProvider();
        }
    }
}
