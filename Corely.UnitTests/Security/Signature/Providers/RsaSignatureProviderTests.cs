using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class RsaSignatureProviderTests : SignatureProviderGenericTests
    {
        private readonly RsaSignatureProvider _rsaSignatureProvider = new(HashAlgorithmName.SHA256);

        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(SignatureConstants.RSA_CODE, _rsaSignatureProvider.SignatureTypeCode);
        }

        public override ISignatureProvider GetSignatureProvider(string expectedVerifyKey)
        {
            return new RsaSignatureProvider(HashAlgorithmName.SHA256);
        }

        public override IAsymmetricSignatureKeyProvider GetKeyProvider()
        {
            return new RsaKeyProvider();
        }
    }
}
