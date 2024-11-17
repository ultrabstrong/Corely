using AutoFixture;
using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using Microsoft.IdentityModel.Tokens;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class SymmetricSignatureProviderBaseTests : SymmetricSignatureProviderGenericTests
    {
        private class MockSymmetricKeyProvider : ISymmetricKeyProvider
        {
            private readonly Fixture _fixture = new();

            public string CreateKey()
            {
                return _fixture.Create<string>();
            }

            public bool IsKeyValid(string key) => true;
        }

        private class MockSignatureProvider : SymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            private readonly MockSymmetricKeyProvider _mockKeyProvider = new();

            private string lastValue = string.Empty;
            private string lastSignature = string.Empty;

            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => _mockKeyProvider;
            public override SigningCredentials GetSigningCredentials(string key) => null!;

            protected override string SignInternal(string value, string key)
            {
                lastValue = value;
                lastSignature = $"{value}{key}";
                return lastSignature;
            }

            protected override bool VerifyInternal(string value, string signature, string key) =>
                lastValue == value
                && lastSignature == signature
                && lastSignature.EndsWith(key);
        }

        private abstract class MockSymmetricSignatureProviderBase : SymmetricSignatureProviderBase
        {
            public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
            public override SigningCredentials GetSigningCredentials(string key) => null!;
            protected override string SignInternal(string value, string key) => value;
            protected override bool VerifyInternal(string value, string signature, string key) => false;
        }

        private class NullMockSignatureProvider : MockSymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => null!;
        }

        private class EmptyMockSignatureProvider : MockSymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => string.Empty;
        }

        private class WhitespaceMockSignatureProvider : MockSymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => " ";
        }

        private class ColonMockSignatureProvider : MockSymmetricSignatureProviderBase
        {
            public override string SignatureTypeCode => "as:df";
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockSignatureProvider _mockSignatureProvider = new();

        [Fact]
        public void NullEncryptionTypeCode_Throws_OnBuild()
        {
            var ex = Record.Exception(() => new NullMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void EmptyEncryptionTypeCode_Throws_OnBuild()
        {
            var ex = Record.Exception(() => new EmptyMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void WhitespaceEncryptionTypeCode_Throws_OnBuild()
        {
            var ex = Record.Exception(() => new WhitespaceMockSignatureProvider());
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ColonEncryptionTypeCode_Throws_OnBuild()
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
        public override void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _mockSignatureProvider.GetSymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<MockSymmetricKeyProvider>(keyProvider);
        }

        public override ISymmetricSignatureProvider GetSignatureProvider()
        {
            return new MockSignatureProvider();
        }
    }
}
