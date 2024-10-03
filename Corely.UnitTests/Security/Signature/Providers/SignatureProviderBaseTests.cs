using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public sealed class SignatureProviderBaseTests : SignatureProviderGenericTests
    {
        private class MockSignatureProvider : SignatureProviderBase
        {
            public override string SignatureTypeCode => TEST_ENCRYPTION_TYPE_CODE;

            private readonly string _expectedVerifyKey;

            public MockSignatureProvider(string expectedVerifyKey)
            {
                _expectedVerifyKey = expectedVerifyKey;
            }

            private string lastValue = string.Empty;
            private string lastSignature = string.Empty;

            protected override string SignInternal(string value, string privateKey)
            {
                lastValue = value;
                lastSignature = $"{Guid.NewGuid()}{value}";
                return lastSignature;
            }

            protected override bool VerifyInternal(string value, string signature, string publicKey) =>
                lastValue == value
                && lastSignature == signature
                && _expectedVerifyKey == publicKey;
        }

        private class NullMockSignatureProvider : SignatureProviderBase
        {
            public override string SignatureTypeCode => null!;
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private class EmptyMockSignatureProvider : SignatureProviderBase
        {
            public override string SignatureTypeCode => string.Empty;
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private class WhitespaceMockSignatureProvider : SignatureProviderBase
        {
            public override string SignatureTypeCode => " ";
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private class ColonMockSignatureProvider : SignatureProviderBase
        {
            public override string SignatureTypeCode => "as:df";
            protected override string SignInternal(string value, string privateKey) => value;
            protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockSignatureProvider _mockSignatureProvider = new(string.Empty);

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

        public override ISignatureProvider GetSignatureProvider(string expectedVerifyKey)
        {
            return new MockSignatureProvider(expectedVerifyKey);
        }

        public override IAsymmetricSignatureKeyProvider GetKeyProvider()
        {
            return new RsaKeyProvider();
        }
    }
}
