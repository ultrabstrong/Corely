using System.Security.Cryptography;

namespace Corely.Common.Providers.Security.Hashing
{
    internal sealed class Sha512SaltedHashProvider : SaltedHashProviderBase
    {
        public override string HashTypeCode => HashProviderConstants.SALTED_SHA512;

        protected override byte[] HashInternal(byte[] value)
        {
            return SHA512.HashData(value);
        }
    }
}
