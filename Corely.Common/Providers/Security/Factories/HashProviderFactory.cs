using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Hashing;

namespace Corely.Common.Providers.Security.Factories
{
    public class HashProviderFactory : IHashProviderFactory
    {
        private readonly Dictionary<string, IHashProvider> _providers = new()
        {
            { HashProviderConstants.SALTED_SHA256_CODE, new Sha256SaltedHashProvider() },
            { HashProviderConstants.SALTED_SHA512_CODE, new Sha512SaltedHashProvider() }
        };
        private readonly string _defaultProviderCode;

        public HashProviderFactory(string defaultProviderCode)
        {
            _defaultProviderCode = defaultProviderCode;
        }

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

        public IHashProvider GetDefaultProvider() => GetProvider(_defaultProviderCode);

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
            return GetProvider(parts[0]);
        }

        public List<(string providerCode, Type providerType)> ListProviders()
        {
            return _providers
                .Select(kvp => (
                    providerCode: kvp.Key,
                    providerType: kvp.Value.GetType()))
                .ToList();
        }
    }
}
