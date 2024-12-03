using Corely.Security.Signature.Providers;
using System.Security.Cryptography;

namespace Corely.Security.Signature.Factories;

public class AsymmetricSignatureProviderFactory : IAsymmetricSignatureProviderFactory
{
    private readonly string _defaultProviderCode;
    private readonly Dictionary<string, IAsymmetricSignatureProvider> _providers = [];

    public AsymmetricSignatureProviderFactory(string defaultProviderCode)
    {
        ArgumentNullException.ThrowIfNull(defaultProviderCode, nameof(defaultProviderCode));

        _defaultProviderCode = defaultProviderCode;
        _providers.Add(AsymmetricSignatureConstants.ECDSA_SHA256_CODE, new ECDsaSignatureProvider(HashAlgorithmName.SHA256));
        _providers.Add(AsymmetricSignatureConstants.RSA_SHA256_CODE, new RsaSignatureProvider(HashAlgorithmName.SHA256));
    }
    public void AddProvider(string providerCode, IAsymmetricSignatureProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        Validate(providerCode);

        if (_providers.ContainsKey(providerCode))
        {
            throw new SignatureException($"Asymmetric signature provider code already exists: {providerCode}")
            {
                Reason = SignatureException.ErrorReason.InvalidTypeCode
            };
        }

        _providers.Add(providerCode, provider);
    }

    public void UpdateProvider(string providerCode, IAsymmetricSignatureProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider, nameof(provider));

        Validate(providerCode);

        if (!_providers.ContainsKey(providerCode))
        {
            throw new SignatureException($"Asymmetric signature provider code not found: {providerCode}")
            {
                Reason = SignatureException.ErrorReason.InvalidTypeCode
            };
        }

        _providers[providerCode] = provider;
    }

    private static void Validate(string providerCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));
        if (providerCode.Contains(':'))
        {
            throw new SignatureException($"Asymmetric signature type code cannot contain ':'")
            {
                Reason = SignatureException.ErrorReason.InvalidTypeCode
            };
        }
    }

    public IAsymmetricSignatureProvider GetDefaultProvider() => GetProvider(_defaultProviderCode);

    public IAsymmetricSignatureProvider GetProvider(string providerCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

        if (!_providers.TryGetValue(providerCode, out IAsymmetricSignatureProvider? value))
        {
            throw new SignatureException($"Asymmetric signature provider code not found: {providerCode}")
            {
                Reason = SignatureException.ErrorReason.InvalidTypeCode
            };
        }

        return value;
    }

    public IAsymmetricSignatureProvider GetProviderForVerifying(string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var providerCode = value.Split(':')[0];
        return GetProvider(providerCode);
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
