using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using Microsoft.IdentityModel.Tokens;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class HmacSha256SignatureProviderTests : SymmetricSignatureProviderGenericTests
    {
        private readonly HmacSha256SignatureProvider _hmacSha256SignatureProvider = new();

        [Fact]
        public override void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            Assert.Equal(SymmetricSignatureConstants.HMAC_SHA256_CODE, _hmacSha256SignatureProvider.SignatureTypeCode);
        }

        [Fact]
        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            var keyProvider = _hmacSha256SignatureProvider.GetSymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<RandomKeyProvider>(keyProvider);
        }

        [Fact]
        public void GetSigningCredentials_ReturnsCorrectSigningCredentials_ForImplementation()
        {
            var key = _hmacSha256SignatureProvider.GetSymmetricKeyProvider().CreateKey();
            var signingCredentials = _hmacSha256SignatureProvider.GetSigningCredentials(key);
            Assert.NotNull(signingCredentials);
            Assert.Equal(SecurityAlgorithms.HmacSha256, signingCredentials.Algorithm);
            Assert.IsType<SymmetricSecurityKey>(signingCredentials.Key);
        }

        public override ISymmetricSignatureProvider GetSignatureProvider()
        {
            return new HmacSha256SignatureProvider();
        }
    }
}
