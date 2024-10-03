using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public class RsaEncryptionProviderTests : AsymmetricEncryptionProviderGenericTests
    {
        private readonly RsaEncryptionProvider _rsaEncryptionProvider = new(RSAEncryptionPadding.OaepSHA256);

        public override void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(AsymmetricEncryptionConstants.RSA_CODE, _rsaEncryptionProvider.EncryptionTypeCode);
        }

        public override IAsymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new RsaEncryptionProvider(RSAEncryptionPadding.OaepSHA256);
        }
    }
}
