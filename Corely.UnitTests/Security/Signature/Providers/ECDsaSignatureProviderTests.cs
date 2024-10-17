using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class ECDsaSignatureProviderTests : AsymmetricSignatureProviderGenericTests
    {
        private readonly ECDsaSignatureProvider _ecDsaSignatureProvider = new(HashAlgorithmName.SHA256);

        [Fact]
        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(AsymmetricSignatureConstants.ECDSA_SHA256_CODE, _ecDsaSignatureProvider.SignatureTypeCode);
        }

        [Fact]
        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _ecDsaSignatureProvider.GetAsymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<EcdsaKeyProvider>(keyProvider);
        }

        [Fact]
        public void GetSigningCredentials_ReturnsCorrectSigningCredentials_ForImplementation()
        {
            var (_, privateKey) = _ecDsaSignatureProvider.GetAsymmetricKeyProvider().CreateKeys();

            var signingCredentials = _ecDsaSignatureProvider.GetSigningCredentials(privateKey);
            Assert.NotNull(signingCredentials);
            Assert.Equal(SecurityAlgorithms.EcdsaSha256, signingCredentials.Algorithm);
            Assert.IsType<ECDsaSecurityKey>(signingCredentials.Key);
        }

        public override IAsymmetricSignatureProvider GetSignatureProvider()
        {
            return new ECDsaSignatureProvider(HashAlgorithmName.SHA256);
        }
    }
}
