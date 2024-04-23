using System.Security.Cryptography;

namespace Corely.Security.Hashing.Providers
{
    internal sealed class Sha256SaltedHashProvider : SaltedHashProviderBase
    {
        public override string HashTypeCode => HashConstants.SALTED_SHA256_CODE;

        protected override byte[] HashInternal(byte[] value)
        {
            return SHA256.HashData(value);
        }
    }
}
