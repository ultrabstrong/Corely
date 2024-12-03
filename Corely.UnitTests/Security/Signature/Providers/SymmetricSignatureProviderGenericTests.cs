using AutoFixture;
using Corely.Security.KeyStore;
using Corely.Security.Signature.Providers;

namespace Corely.UnitTests.Security.Signature.Providers;

public abstract class SymmetricSignatureProviderGenericTests
{
    private readonly Fixture _fixture = new();
    private readonly ISymmetricSignatureProvider _signatureProvider;
    private readonly InMemorySymmetricKeyStoreProvider _keyStoreProvider;

    public SymmetricSignatureProviderGenericTests()
    {
        _signatureProvider = GetSignatureProvider();
        var key = _signatureProvider.GetSymmetricKeyProvider().CreateKey();
        _keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);
    }

    [Fact]
    public void Sign_ReturnsAValue()
    {
        var data = _fixture.Create<string>();

        var signature = _signatureProvider.Sign(data, _keyStoreProvider);

        Assert.NotEmpty(signature);
        Assert.NotEqual(data, signature);
    }

    [Fact]
    public void Sign_Throws_WithNullInput()
    {
        var ex = Record.Exception(() => _signatureProvider.Sign(null!, _keyStoreProvider));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void Sign_ThenVerify_ReturnsTrue()
    {
        var data = _fixture.Create<string>();
        var signature = _signatureProvider.Sign(data, _keyStoreProvider);

        var result = _signatureProvider.Verify(data, signature, _keyStoreProvider);

        Assert.True(result);
    }

    [Fact]
    public void Sign_ThenVerify_ReturnsFalse_WithDifferentData()
    {
        var data = _fixture.Create<string>();
        var signature = _signatureProvider.Sign(data, _keyStoreProvider);

        var result = _signatureProvider.Verify(_fixture.Create<string>(), signature, _keyStoreProvider);

        Assert.False(result);
    }

    [Fact]
    public void Sign_ThenVerify_ReturnsFalse_WithDifferentSignature()
    {
        var data = _fixture.Create<string>();
        var otherData = _fixture.Create<string>();
        var otherSignature = _signatureProvider.Sign(otherData, _keyStoreProvider);

        var result = _signatureProvider.Verify(data, otherSignature, _keyStoreProvider);

        Assert.False(result);
    }

    [Fact]
    public void Sign_ThenVerify_ReturnsFalse_WithDifferentKey()
    {
        var data = _fixture.Create<string>();
        var signature = _signatureProvider.Sign(data, _keyStoreProvider);
        var key = _signatureProvider.GetSymmetricKeyProvider().CreateKey();
        var otherKeyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

        var result = _signatureProvider.Verify(data, signature, otherKeyStoreProvider);

        Assert.False(result);
    }

    [Fact]
    public void Verify_Throws_WithNullData()
    {
        var ex = Record.Exception(() => _signatureProvider.Verify(null!, _fixture.Create<string>(), _keyStoreProvider));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public abstract void SignatureTypeCode_ReturnsCorrectCode_ForImplementation();

    [Fact]
    public abstract void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation();

    public abstract ISymmetricSignatureProvider GetSignatureProvider();
}
