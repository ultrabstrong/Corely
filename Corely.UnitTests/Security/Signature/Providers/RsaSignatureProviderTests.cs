using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers;

public class RsaSignatureProviderTests : AsymmetricSignatureProviderGenericTests
{
    private readonly RsaSignatureProvider _rsaSignatureProvider = new(HashAlgorithmName.SHA256);

    [Fact]
    public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
    {
        Assert.Equal(AsymmetricSignatureConstants.RSA_SHA256_CODE, _rsaSignatureProvider.SignatureTypeCode);
    }

    [Fact]
    public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
    {
        var keyProvider = _rsaSignatureProvider.GetAsymmetricKeyProvider();

        Assert.NotNull(keyProvider);
        Assert.IsType<RsaKeyProvider>(keyProvider);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetSigningCredentials_ReturnsCorrectSigningCredentials_ForImplementation(bool isKeyPrivate)
    {
        var (publicKey, privateKey) = _rsaSignatureProvider.GetAsymmetricKeyProvider().CreateKeys();

        var signingCredentials = isKeyPrivate
            ? _rsaSignatureProvider.GetSigningCredentials(privateKey, true)
            : _rsaSignatureProvider.GetSigningCredentials(publicKey, false);

        Assert.NotNull(signingCredentials);
        Assert.Equal(SecurityAlgorithms.RsaSha256, signingCredentials.Algorithm);
        Assert.IsType<RsaSecurityKey>(signingCredentials.Key);
    }

    public override IAsymmetricSignatureProvider GetSignatureProvider()
    {
        return new RsaSignatureProvider(HashAlgorithmName.SHA256);
    }
}
