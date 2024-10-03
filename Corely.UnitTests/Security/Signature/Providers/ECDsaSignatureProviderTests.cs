using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class ECDsaSignatureProviderTests : AsymmetricSignatureProviderGenericTests
    {
        private readonly ECDsaSignatureProvider _ecDsaSignatureProvider = new(HashAlgorithmName.SHA256);

        [Fact]
        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(SignatureConstants.ECDSA_CODE, _ecDsaSignatureProvider.SignatureTypeCode);
        }

        [Fact]
        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _ecDsaSignatureProvider.GetAsymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<EcdsaKeyProvider>(keyProvider);
        }

        public override IAsymmetricSignatureProvider GetSignatureProvider()
        {
            return new ECDsaSignatureProvider(HashAlgorithmName.SHA256);
        }
    }
}
