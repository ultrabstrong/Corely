using System.Security.Cryptography;

namespace Corely.Security.Hashing.Providers
{
    internal sealed class Sha512SaltedHashProvider : SaltedHashProviderBase
    {
        public override string HashTypeCode => HashConstants.SALTED_SHA512_CODE;

        protected override byte[] HashInternal(byte[] value)
        {
            return SHA512.HashData(value);
        }
    }
}
