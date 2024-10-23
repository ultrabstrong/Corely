using Corely.Security.Signature.Providers;

namespace Corely.Security.Signature.Factories
{
    public class SymmetricSignatureProviderFactory : ISymmetricSignatureProviderFactory
    {
        private readonly string _defaultProviderCode;
        private readonly Dictionary<string, ISymmetricSignatureProvider> _providers = [];

        public SymmetricSignatureProviderFactory(string defaultProviderCode)
        {
            ArgumentNullException.ThrowIfNull(defaultProviderCode, nameof(defaultProviderCode));

            _defaultProviderCode = defaultProviderCode;
            _providers.Add(SymmetricSignatureConstants.HMAC_SHA256_CODE, new HmacSha256SignatureProvider());
        }

        public void AddProvider(string providerCode, ISymmetricSignatureProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            Validate(providerCode);

            if (_providers.ContainsKey(providerCode))
            {
                throw new SignatureException($"Symmetric signature provider code already exists: {providerCode}")
                {
                    Reason = SignatureException.ErrorReason.InvalidTypeCode
                };
            }

            _providers.Add(providerCode, provider);
        }

        public void UpdateProvider(string providerCode, ISymmetricSignatureProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            Validate(providerCode);

            if (!_providers.ContainsKey(providerCode))
            {
                throw new SignatureException($"Symmetric signature provider code not found: {providerCode}")
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
                throw new SignatureException($"Symmetric signature type code cannot contain ':'")
                {
                    Reason = SignatureException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public ISymmetricSignatureProvider GetDefaultProvider() => GetProvider(_defaultProviderCode);

        public ISymmetricSignatureProvider GetProvider(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

            if (!_providers.TryGetValue(providerCode, out ISymmetricSignatureProvider? value))
            {
                throw new SignatureException($"Symmetric signature provider code not found: {providerCode}")
                {
                    Reason = SignatureException.ErrorReason.InvalidTypeCode
                };
            }

            return value;
        }

        public ISymmetricSignatureProvider GetProviderForVerifying(string value)
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
}
