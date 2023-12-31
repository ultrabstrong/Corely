using System.Security.Cryptography;

namespace Corely.Common.Providers.Security.Hashing
{
    internal sealed class Sha256SaltedHashProvider : SaltedHashProviderBase
    {
        public override string HashTypeCode => HashProviderConstants.SALTED_SHA256_CODE;

        protected override byte[] HashInternal(byte[] value)
        {
            return SHA256.HashData(value);
        }
    }
}
