using Corely.Security.Hashing.Providers;

namespace Corely.Security.Hashing.Factories;

public class HashProviderFactory : IHashProviderFactory
{
    private readonly Dictionary<string, IHashProvider> _providers = new()
        {
            { HashConstants.SALTED_SHA256_CODE, new Sha256SaltedHashProvider() },
            { HashConstants.SALTED_SHA512_CODE, new Sha512SaltedHashProvider() }
        };
    private readonly string _defaultProviderCode;

    public HashProviderFactory(string defaultProviderCode)
    {
        _defaultProviderCode = defaultProviderCode;
    }

    public void AddProvider(string providerCode, IHashProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));
        Validate(providerCode);

        if (_providers.ContainsKey(providerCode))
        {
            throw new HashException($"Hash provider code already exists: {providerCode}")
            {
                Reason = HashException.ErrorReason.InvalidTypeCode
            };
        }

        _providers.Add(providerCode, provider);
    }

    public void UpdateProvider(string providerCode, IHashProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));
        Validate(providerCode);

        if (!_providers.ContainsKey(providerCode))
        {
            throw new HashException($"Hash provider code not found: {providerCode}")
            {
                Reason = HashException.ErrorReason.InvalidTypeCode
            };
        }

        _providers[providerCode] = provider;
    }

    private static void Validate(string providerCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));
        if (providerCode.Contains(':'))
        {
            throw new HashException($"Hash type code cannot contain ':'")
            {
                Reason = HashException.ErrorReason.InvalidTypeCode
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

        throw new HashException($"Hash provider code unknown: {providerCode}")
        {
            Reason = HashException.ErrorReason.InvalidTypeCode
        };
    }

    public IHashProvider GetProviderToVerify(string hash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hash, nameof(hash));

        string[] parts = hash.Split(':');
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
