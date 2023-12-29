using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public class HashProviderFactory : IHashProviderFactory
    {
        private readonly Dictionary<string, IHashProvider> _providers = new()
        {
            { HashProviderConstants.SALTED_SHA256, new Sha256SaltedHashProvider() },
            { HashProviderConstants.SALTED_SHA512, new Sha512SaltedHashProvider() }
        };

        public void AddProvider(string providerCode, IHashProvider provider)
        {
            provider.ThrowIfNull(nameof(provider));
            Validate(providerCode);

            if (_providers.ContainsKey(providerCode))
            {
                throw new HashProviderException($"Hash provider code already exists: {providerCode}")
                {
                    Reason = HashProviderException.ErrorReason.InvalidTypeCode
                };
            }

            _providers.Add(providerCode, provider);
        }

        public void UpdateProvider(string providerCode, IHashProvider provider)
        {
            provider.ThrowIfNull(nameof(provider));
            Validate(providerCode);

            if (!_providers.ContainsKey(providerCode))
            {
                throw new HashProviderException($"Hash provider code not found: {providerCode}")
                {
                    Reason = HashProviderException.ErrorReason.InvalidTypeCode
                };
            }

            _providers[providerCode] = provider;
        }

        private static void Validate(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));
            if (providerCode.Contains(':'))
            {
                throw new HashProviderException($"Hash type code cannot contain ':'")
                {
                    Reason = HashProviderException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public IHashProvider GetProvider(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

            if (_providers.TryGetValue(providerCode, out IHashProvider? provider))
            {
                return provider;
            }

            throw new HashProviderException($"Hash provider code unknown: {providerCode}")
            {
                Reason = HashProviderException.ErrorReason.InvalidTypeCode
            };
        }

        public IHashProvider GetProviderToVerify(string hash)
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

            return GetProvider(parts[0]);
        }

        public List<(string ProviderCode, Type ProviderType)> ListProviders()
        {
            return _providers
                .Select(kvp => (
                    ProviderCode: kvp.Key,
                    ProviderType: kvp.Value.GetType()))
                .ToList();
        }
    }
}
