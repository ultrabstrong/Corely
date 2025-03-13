using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;

namespace Corely.Security.UnitTests.Encryption.Providers;

public sealed class SymmetricEncryptionProviderBaseTests : SymmetricEncryptionProviderGenericTests
{
    private class MockSymmetricKeyProvider : ISymmetricKeyProvider
    {
        public string CreateKey() => string.Empty;
        public bool IsKeyValid(string key) => true;
    }

    private class MockEncryptionProvider : SymmetricEncryptionProviderBase
    {
        public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
        private readonly MockSymmetricKeyProvider _mockKeyProvider = new();
        public override ISymmetricKeyProvider GetSymmetricKeyProvider() => _mockKeyProvider;
        protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
        protected override string DecryptInternal(string value, string key) => value[36..];
    }

    private abstract class MockSymmetricEncryptionProviderBase : SymmetricEncryptionProviderBase
    {
        public override ISymmetricKeyProvider GetSymmetricKeyProvider() => null!;
        protected override string EncryptInternal(string value, string key) => value;
        protected override string DecryptInternal(string value, string key) => value;
    }

    private class NullMockEncryptionProvider : MockSymmetricEncryptionProviderBase
    {
        public override string EncryptionTypeCode => null!;
    }

    private class EmptyMockEncryptionProvider : MockSymmetricEncryptionProviderBase
    {
        public override string EncryptionTypeCode => string.Empty;
    }

    private class WhitespaceMockEncryptionProvider : MockSymmetricEncryptionProviderBase
    {
        public override string EncryptionTypeCode => " ";
    }

    private class ColonMockEncryptionProvider : MockSymmetricEncryptionProviderBase
    {
        public override string EncryptionTypeCode => "as:df";
    }

    private const string TEST_ENCRYPTION_TYPE_CODE = "00";

    private readonly MockEncryptionProvider _mockEncryptionProvider = new();

    [Fact]
    public void NullEncryptionTypeCode_Throws_OnBuild()
    {
        var ex = Record.Exception(() => new NullMockEncryptionProvider());
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void EmptyEncryptionTypeCode_Throws_OnBuild()
    {
        var ex = Record.Exception(() => new EmptyMockEncryptionProvider());
        Assert.NotNull(ex);
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void WhitespaceEncryptionTypeCode_Throws_OnBuild()
    {
        var ex = Record.Exception(() => new WhitespaceMockEncryptionProvider());
        Assert.NotNull(ex);
        Assert.IsType<ArgumentException>(ex);
    }

    [Fact]
    public void ColonEncryptionTypeCode_Throws_OnBuild()
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
    public override void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
    {
        var keyProvider = _mockEncryptionProvider.GetSymmetricKeyProvider();

        Assert.NotNull(keyProvider);
        Assert.IsType<MockSymmetricKeyProvider>(keyProvider);
    }

    public override ISymmetricEncryptionProvider GetEncryptionProvider()
    {
        return new MockEncryptionProvider();
    }
}
