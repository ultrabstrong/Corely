using Corely.Security.Encryption.Providers;

namespace Corely.Security.Encryption.Factories;

public class SymmetricEncryptionProviderFactory : ISymmetricEncryptionProviderFactory
{
    private readonly string _defaultProviderCode;
    private readonly Dictionary<string, ISymmetricEncryptionProvider> _providers = [];

    public SymmetricEncryptionProviderFactory(string defaultProviderCode)
    {
        ArgumentNullException.ThrowIfNull(defaultProviderCode, nameof(defaultProviderCode));

        _defaultProviderCode = defaultProviderCode;
        _providers.Add(SymmetricEncryptionConstants.AES_CODE, new AesEncryptionProvider());
    }

    public void AddProvider(string providerCode, ISymmetricEncryptionProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        Validate(providerCode);

        if (_providers.ContainsKey(providerCode))
        {
            throw new EncryptionException($"Symmetric encryption provider code already exists: {providerCode}")
            {
                Reason = EncryptionException.ErrorReason.InvalidTypeCode
            };
        }

        _providers.Add(providerCode, provider);
    }

    public void UpdateProvider(string providerCode, ISymmetricEncryptionProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        Validate(providerCode);

        if (!_providers.ContainsKey(providerCode))
        {
            throw new EncryptionException($"Symmetric encryption provider code not found: {providerCode}")
            {
                Reason = EncryptionException.ErrorReason.InvalidTypeCode
            };
        }

        _providers[providerCode] = provider;
    }

    private static void Validate(string providerCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));
        if (providerCode.Contains(':'))
        {
            throw new EncryptionException($"Symmetric encryption type code cannot contain ':'")
            {
                Reason = EncryptionException.ErrorReason.InvalidTypeCode
            };
        }
    }

    public ISymmetricEncryptionProvider GetDefaultProvider() => GetProvider(_defaultProviderCode);

    public ISymmetricEncryptionProvider GetProvider(string providerCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

        if (_providers.TryGetValue(providerCode, out ISymmetricEncryptionProvider? value))
        {
            return value;
        }

        throw new EncryptionException($"Symmetric encryption provider code unknown: {providerCode}")
        {
            Reason = EncryptionException.ErrorReason.InvalidTypeCode
        };
    }

    public ISymmetricEncryptionProvider GetProviderForDecrypting(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

        string[] parts = value.Split(':');
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
