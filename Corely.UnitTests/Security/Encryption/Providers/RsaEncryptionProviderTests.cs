using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public class RsaEncryptionProviderTests : AsymmetricEncryptionProviderGenericTests
    {
        private readonly RsaEncryptionProvider _rsaEncryptionProvider = new(RSAEncryptionPadding.OaepSHA256);

        [Fact]
        public override void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(AsymmetricEncryptionConstants.RSA_CODE, _rsaEncryptionProvider.EncryptionTypeCode);
        }

        [Fact]
        public override void GetAsymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
        {
            var keyProvider = _rsaEncryptionProvider.GetAsymmetricKeyProvider();

            Assert.NotNull(keyProvider);
            Assert.IsType<RsaKeyProvider>(keyProvider);
        }
        public override IAsymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new RsaEncryptionProvider(RSAEncryptionPadding.OaepSHA256);
        }
    }
}
