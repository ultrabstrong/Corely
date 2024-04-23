using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Factories
{
    public class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        private readonly string _defaultProviderCode;
        private readonly IKeyStoreProvider _keyStoreProvider;
        private readonly Dictionary<string, IEncryptionProvider> _providers = [];

        public EncryptionProviderFactory(string defaultProviderCode, IKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(defaultProviderCode, nameof(defaultProviderCode));
            ArgumentNullException.ThrowIfNull(keyStoreProvider, nameof(keyStoreProvider));

            _defaultProviderCode = defaultProviderCode;
            _keyStoreProvider = keyStoreProvider;
            _providers.Add(EncryptionConstants.AES_CODE, new AesEncryptionProvider(_keyStoreProvider));
        }

        public void AddProvider(string providerCode, IEncryptionProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            Validate(providerCode);

            if (_providers.ContainsKey(providerCode))
            {
                throw new EncryptionException($"Encryption provider code already exists: {providerCode}")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }

            _providers.Add(providerCode, provider);
        }

        public void UpdateProvider(string providerCode, IEncryptionProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider, nameof(provider));

            Validate(providerCode);

            if (!_providers.ContainsKey(providerCode))
            {
                throw new EncryptionException($"Encryption provider code not found: {providerCode}")
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
                throw new EncryptionException($"Encryption type code cannot contain ':'")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public IEncryptionProvider GetDefaultProvider() => GetProvider(_defaultProviderCode);

        public IEncryptionProvider GetProvider(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

            if (_providers.TryGetValue(providerCode, out IEncryptionProvider? value))
            {
                return value;
            }

            throw new EncryptionException($"Encryption provider code unknown: {providerCode}")
            {
                Reason = EncryptionException.ErrorReason.InvalidTypeCode
            };
        }

        public IEncryptionProvider GetProviderForDecrypting(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            string[] parts = value.Split(':');
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
