using AutoFixture;
using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using Microsoft.IdentityModel.Tokens;

namespace Corely.Security.UnitTests.Signature.Providers;

public sealed class AsymmetricSignatureProviderBaseTests : AsymmetricSignatureProviderGenericTests
{
    private class MockAsymmetricKeyProvider : IAsymmetricKeyProvider
    {
        private readonly Fixture _fixture = new();

        public (string PublicKey, string PrivateKey) CreateKeys()
        {
            var key = _fixture.Create<string>();
            return (key, key); // This allows mocking signature verification success / failure
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

        public override SigningCredentials GetSigningCredentials(string key, bool isKeyPrivate) => null!;

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

    private abstract class MockAsymmetricSignatureProviderBase : AsymmetricSignatureProviderBase
    {
        public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => null!;
        public override SigningCredentials GetSigningCredentials(string key, bool isKeyPrivate) => null!;
        protected override string SignInternal(string value, string privateKey) => value;
        protected override bool VerifyInternal(string value, string signature, string publicKey) => false;
    }

    private class NullMockSignatureProvider : MockAsymmetricSignatureProviderBase
    {
        public override string SignatureTypeCode => null!;
    }

    private class EmptyMockSignatureProvider : MockAsymmetricSignatureProviderBase
    {
        public override string SignatureTypeCode => string.Empty;
    }

    private class WhitespaceMockSignatureProvider : MockAsymmetricSignatureProviderBase
    {
        public override string SignatureTypeCode => " ";
    }

    private class ColonMockSignatureProvider : MockAsymmetricSignatureProviderBase
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
