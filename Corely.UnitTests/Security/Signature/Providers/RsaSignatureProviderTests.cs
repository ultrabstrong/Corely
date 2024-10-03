using Corely.Security.Keys;
using Corely.Security.Signature;
using Corely.Security.Signature.Providers;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public class RsaSignatureProviderTests : AsymmetricSignatureProviderGenericTests
    {
        private readonly RsaSignatureProvider _rsaSignatureProvider = new(HashAlgorithmName.SHA256);

        [Fact]
        public override void SignatureTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(SignatureConstants.RSA_CODE, _rsaSignatureProvider.SignatureTypeCode);
        }

        [Fact]
        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _rsaSignatureProvider.GetAsymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<RsaKeyProvider>(keyProvider);
        }

        public override IAsymmetricSignatureProvider GetSignatureProvider()
        {
            return new RsaSignatureProvider(HashAlgorithmName.SHA256);
        }
    }
}
