using AutoFixture;
using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public sealed class AsymmetricSignatureProviderBaseTests : AsymmetricSignatureProviderGenericTests
    {
        private class MockAsymmetricKeyProvider : IAsymmetricKeyProvider
        {
            private readonly Fixture _fixture = new();

            public (string PublicKey, string PrivateKey) CreateKeys()
            {
                var key = _fixture.Create<string>();
                return (key, key); // This allows us to mock signature verification success / failure
            }

            public bool IsKeyValid(string publicKey, string privateKey) => true;
        }

        private class MockSignatureProvider : AsymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            private readonly MockAsymmetricKeyProvider _mockKeyProvider = new();

            private string lastValue = string.Empty;
            private string lastSignature = string.Empty;

            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => _mockKeyProvider;

            protected override string SignInternal(string value, string privateKey)
            {
                lastValue = value;
                lastSignature = $"{value}{privateKey}";
                return lastSignature;
            }

            protected override bool VerifyInternal(string value, string signature, string publicKey) =>
                lastValue == value
                && lastSignature == signature
                && lastSignature.EndsWith(publicKey);
        }

        private class NullMockSignatureProvider : AsymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => null!;
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private class EmptyMockSignatureProvider : AsymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => string.Empty;
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private class WhitespaceMockSignatureProvider : AsymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => " ";
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private class ColonMockSignatureProvider : AsymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => "as:df";
            public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockSignatureProvider _mockSignatureProvider = new();

        [Fact]
        public void NullEncryptionTypeCode_ThrowsArgumentNullException_OnBuild()
        {
            var ex = Record.Exception(() => new NullMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void EmptyEncryptionTypeCode_ThrowsArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new EmptyMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void WhitespaceEncryptionTypeCode_ThrowsArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new WhitespaceMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ColonEncryptionTypeCode_ThrowsArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new ColonMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<SignatureException>(ex);
        }

        [Fact]
        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(TEST_ENCRYPTION_TYPE_CODE, _mockSignatureProvider.SignatureTypeCode);
        }

        [Fact]
        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _mockSignatureProvider.GetAsymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<MockAsymmetricKeyProvider>(keyProvider);
        }

        public override IAsymmetricSignatureProvider GetSignatureProvider()
        {
            return new MockSignatureProvider();
        }
    }
}
