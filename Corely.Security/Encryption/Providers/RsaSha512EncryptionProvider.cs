using System.Security.Cryptography;

namespace Corely.Security.Encryption.Providers
{
    internal sealed class RsaSha512EncryptionProvider : RsaEncryptionProviderBase
    {
        protected override RSAEncryptionPadding RsaEncryptionPadding => RSAEncryptionPadding.OaepSHA512;

        public override string EncryptionTypeCode => AsymmetricEncryptionConstants.RSA_SHA512_CODE;
    }
}
