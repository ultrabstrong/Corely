using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class RsaSignatureProviderTests : AsymmetricSignatureProviderGenericTests
    {
        private readonly RsaSignatureProvider _rsaSignatureProvider = new(HashAlgorithmName.SHA256);

        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(SignatureConstants.RSA_CODE, _rsaSignatureProvider.SignatureTypeCode);
        }

        public override IAsymmetricSignatureProvider GetSignatureProvider(string expectedVerifyKey)
        {
            return new RsaSignatureProvider(HashAlgorithmName.SHA256);
        }

        public override IAsymmetricKeyProvider GetKeyProvider()
        {
            return new RsaKeyProvider();
        }
    }
}
