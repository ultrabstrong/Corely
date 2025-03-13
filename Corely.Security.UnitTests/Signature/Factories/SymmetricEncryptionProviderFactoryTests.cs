using AutoFixture;
using Corely.Security.Signature;
using Corely.Security.Signature.Factories;
using Corely.Security.Signature.Providers;
using Corely.TestBase.ClassData;

namespace Corely.Security.UnitTests.Signature.Factories;

public class SymmetricEncryptionProviderFactoryTests
{
    private const string DEFAULT_PROVIDER_CODE = SymmetricSignatureConstants.HMAC_SHA256_CODE;
    private readonly SymmetricSignatureProviderFactory _signatureProviderFactory = new(DEFAULT_PROVIDER_CODE);
    private readonly Fixture _fixture = new();

    [Fact]
    public void AddProvider_AddsProvider()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<ISymmetricSignatureProvider>().Object;

        _signatureProviderFactory.AddProvider(providerCode, provider);
        var signatureProvider = _signatureProviderFactory.GetProvider(providerCode);

        Assert.NotNull(signatureProvider);
    }

    [Fact]
    public void AddProvider_Throws_WithExistingProviderCode()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<ISymmetricSignatureProvider>().Object;

        _signatureProviderFactory.AddProvider(providerCode, provider);
        var ex = Record.Exception(() => _signatureProviderFactory.AddProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.IsType<SignatureException>(ex);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData(":")]
    public void AddProvider_Throws_WithInvalidCode(string providerCode)
    {
        var provider = new Mock<ISymmetricSignatureProvider>().Object;
        var ex = Record.Exception(() => _signatureProviderFactory.AddProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is SignatureException);
    }

    [Fact]
    public void AddProvider_Throws_WithNullProvider()
    {
        var providerCode = _fixture.Create<string>();

        var ex = Record.Exception(() => _signatureProviderFactory.AddProvider(providerCode, null));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void UpdateProvider_UpdatesProvider()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<ISymmetricSignatureProvider>().Object;
        var updatedProvider = new Mock<ISymmetricSignatureProvider>().Object;

        _signatureProviderFactory.AddProvider(providerCode, provider);
        _signatureProviderFactory.UpdateProvider(providerCode, updatedProvider);
        var signatureProvider = _signatureProviderFactory.GetProvider(providerCode);

        Assert.Same(updatedProvider, signatureProvider);
    }

    [Fact]
    public void UpdateProvider_Throws_WithNonExistingProviderCode()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<ISymmetricSignatureProvider>().Object;

        var ex = Record.Exception(() => _signatureProviderFactory.UpdateProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.IsType<SignatureException>(ex);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData(":")]
    public void UpdateProvider_Throws_WithInvalidCode(string providerCode)
    {
        var provider = new Mock<ISymmetricSignatureProvider>().Object;
        var ex = Record.Exception(() => _signatureProviderFactory.UpdateProvider(providerCode, provider));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is SignatureException);
    }

    [Fact]
    public void UpdateProvider_Throws_WithNullProvider()
    {
        var providerCode = _fixture.Create<string>();
        var ex = Record.Exception(() => _signatureProviderFactory.UpdateProvider(providerCode, null));

        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void GetDefaultProvider_ReturnsDefaultProvider()
    {
        var signatureProvider = _signatureProviderFactory.GetDefaultProvider();

        Assert.NotNull(signatureProvider);
        Assert.Equal(DEFAULT_PROVIDER_CODE, signatureProvider.SignatureTypeCode);
    }

    [Theory, MemberData(nameof(GetProviderData))]
    public void GetProvider_ReturnsProvider(string providerCode, Type providerType)
    {
        var signatureProvider = _signatureProviderFactory.GetProvider(providerCode);

        Assert.NotNull(signatureProvider);
        Assert.Equal(providerType, signatureProvider.GetType());
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData("-")]
    [InlineData("--")]
    public void GetProvider_Throws_WithInvalidCode(string providerCode)
    {
        var ex = Record.Exception(() => _signatureProviderFactory.GetProvider(providerCode));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is SignatureException);
    }

    [Theory, MemberData(nameof(GetProviderData))]
    public void GetProviderForVerifying_ReturnsSignatureProvider(string code, Type providerType)
    {
        var signedValue = $"{code}:1:{_fixture.Create<string>()}";
        var signatureProvider = _signatureProviderFactory.GetProviderForVerifying(signedValue);

        Assert.NotNull(signatureProvider);
        Assert.IsType(providerType, signatureProvider);
    }

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    [InlineData("-")]
    [InlineData("--")]
    public void GetProviderForVerifying_Throws_WithInvalidCode(string code)
    {
        var ex = Record.Exception(() => _signatureProviderFactory.GetProviderForVerifying(code));

        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException
            || ex is ArgumentException
            || ex is SignatureException);
    }

    [Fact]
    public void ListProviders_ReturnsProviders()
    {
        var providerCode = _fixture.Create<string>();
        var provider = new Mock<ISymmetricSignatureProvider>().Object;

        var providers = _signatureProviderFactory.ListProviders();
        _signatureProviderFactory.AddProvider(providerCode, provider);
        var updatedProviders = _signatureProviderFactory.ListProviders();

        Assert.NotNull(providers);
        Assert.NotEmpty(providers);
        Assert.NotNull(updatedProviders);
        Assert.NotEmpty(updatedProviders);
        Assert.True(providers.Count < updatedProviders.Count);
    }

    public static IEnumerable<object[]> GetProviderData()
    {
        yield return new object[] { SymmetricSignatureConstants.HMAC_SHA256_CODE, typeof(HmacSha256SignatureProvider) };
    }
}
