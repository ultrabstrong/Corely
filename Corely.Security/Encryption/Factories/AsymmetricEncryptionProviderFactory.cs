using Corely.Security.Encryption.Providers;
using System.Security.Cryptography;

namespace Corely.Security.Encryption.Factories
{
    public class AsymmetricEncryptionProviderFactory : IAsymmetricEncryptionProviderFactory
    {
        private readonly string _defaultProviderCode;
        private readonly Dictionary<string, IAsymmetricEncryptionProvider> _providers = [];

        public AsymmetricEncryptionProviderFactory(string defaultProviderCode)
        {
            ArgumentNullException.ThrowIfNull(defaultProviderCode, nameof(defaultProviderCode));

            _defaultProviderCode = defaultProviderCode;
            _providers.Add(AsymmetricEncryptionConstants.RSA_CODE, new RsaEncryptionProvider(RSAEncryptionPadding.OaepSHA256));
        }

        public void AddProvider(string providerCode, IAsymmetricEncryptionProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            Validate(providerCode);

            if (_providers.ContainsKey(providerCode))
            {
                throw new EncryptionException($"Asymmetric encryption provider code already exists: {providerCode}")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }

            _providers.Add(providerCode, provider);
        }

        public void UpdateProvider(string providerCode, IAsymmetricEncryptionProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            Validate(providerCode);

            if (!_providers.ContainsKey(providerCode))
            {
                throw new EncryptionException($"Asymmetric encryption provider code not found: {providerCode}")
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
                throw new EncryptionException($"Asymmetric encryption type code cannot contain ':'")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public IAsymmetricEncryptionProvider GetDefaultProvider() => GetProvider(_defaultProviderCode);

        public IAsymmetricEncryptionProvider GetProvider(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));
            if (!_providers.TryGetValue(providerCode, out IAsymmetricEncryptionProvider? value))
            {
                throw new EncryptionException($"Asymmetric encryption provider code not found: {providerCode}")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }

            return value;
        }

        public IAsymmetricEncryptionProvider GetProviderForDecrypting(string value)
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
