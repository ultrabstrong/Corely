using AutoFixture;
using Corely.Security.KeyStore;
using Corely.Security.Signature.Providers;

namespace Corely.Security.UnitTests.Signature.Providers;

public abstract class AsymmetricSignatureProviderGenericTests
{
    private readonly Fixture _fixture = new();
    private readonly IAsymmetricSignatureProvider _signatureProvider;
    private readonly InMemoryAsymmetricKeyStoreProvider _keyStoreProvider;

    public AsymmetricSignatureProviderGenericTests()
    {
        _signatureProvider = GetSignatureProvider();
        var (publicKey, privateKey) = _signatureProvider.GetAsymmetricKeyProvider().CreateKeys();
        _keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);
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
        var (publicKey, privateKey) = _signatureProvider.GetAsymmetricKeyProvider().CreateKeys();
        var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);

        var result = _signatureProvider.Verify(data, signature, keyStoreProvider);

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
    public abstract void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation();

    public abstract IAsymmetricSignatureProvider GetSignatureProvider();
}
