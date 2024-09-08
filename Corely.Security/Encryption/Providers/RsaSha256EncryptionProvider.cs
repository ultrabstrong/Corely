using System.Security.Cryptography;

namespace Corely.Security.Encryption.Providers
{
    internal sealed class RsaSha256EncryptionProvider : RsaEncryptionProviderBase
    {
        protected override RSAEncryptionPadding RsaEncryptionPadding => RSAEncryptionPadding.OaepSHA256;

        public override string EncryptionTypeCode => AsymmetricEncryptionConstants.RSA_SHA256_CODE;
    }
}
