using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public class HashProviderFactory : IHashProviderFactory
    {
        public IHashProvider Create(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

            return providerCode switch
            {
                HashProviderConstants.SALTED_SHA256 => new Sha256SaltedHashProvider(),
                _ => throw new HashProviderException($"Unknown hash provider code {providerCode}")
                {
                    Reason = HashProviderException.ErrorReason.InvalidTypeCode
                }
            };
        }

        public IHashProvider CreateToVerify(string hash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(hash, nameof(hash));

            string[] parts = hash.Split(':');
            if (parts.Length < 1)
            {
                throw new HashProviderException($"Hash must be in format hashTypeCode:hashedValue")
                {
                    Reason = HashProviderException.ErrorReason.InvalidFormat
                };
            }

            return Create(parts[0]);
        }
    }
}
