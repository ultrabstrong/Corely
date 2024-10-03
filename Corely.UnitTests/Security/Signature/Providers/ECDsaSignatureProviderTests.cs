using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class ECDsaSignatureProviderTests : AsymmetricSignatureProviderGenericTests
    {
        private readonly ECDsaSignatureProvider _ecDsaSignatureProvider = new(HashAlgorithmName.SHA256);

        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(SignatureConstants.ECDSA_CODE, _ecDsaSignatureProvider.SignatureTypeCode);
        }

        public override IAsymmetricSignatureProvider GetSignatureProvider(string expectedVerifyKey)
        {
            return new ECDsaSignatureProvider(HashAlgorithmName.SHA256);
        }

        public override IAsymmetricKeyProvider GetKeyProvider()
        {
            return new EcdsaKeyProvider();
        }
    }
}
