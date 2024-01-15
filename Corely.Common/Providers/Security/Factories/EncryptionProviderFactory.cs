using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Keys;

namespace Corely.Common.Providers.Security.Factories
{
    public class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        private protected readonly IKeyStoreProvider _keyStoreProvider;
        private protected readonly Dictionary<string, IEncryptionProvider> _providers = [];

        public EncryptionProviderFactory(IKeyStoreProvider keyStoreProvider)
        {
            _keyStoreProvider = keyStoreProvider.ThrowIfNull(nameof(keyStoreProvider));
            _providers.Add(EncryptionProviderConstants.AES_CODE, new AesEncryptionProvider(_keyStoreProvider));
        }

        public void AddProvider(string providerCode, IEncryptionProvider provider)
        {
            provider.ThrowIfNull(nameof(provider));
            Validate(providerCode);

            if (_providers.ContainsKey(providerCode))
            {
                throw new EncryptionProviderException($"Encryption provider code already exists: {providerCode}")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
                };
            }

            _providers.Add(providerCode, provider);
        }

        public void UpdateProvider(string providerCode, IEncryptionProvider provider)
        {
            provider.ThrowIfNull(nameof(provider));
            Validate(providerCode);

            if (!_providers.ContainsKey(providerCode))
            {
                throw new EncryptionProviderException($"Encryption provider code not found: {providerCode}")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
                };
            }

            _providers[providerCode] = provider;
        }

        private static void Validate(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));
            if (providerCode.Contains(':'))
            {
                throw new EncryptionProviderException($"Encryption type code cannot contain ':'")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public IEncryptionProvider GetProvider(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

            if (_providers.TryGetValue(providerCode, out IEncryptionProvider? value))
            {
                return value;
            }

            throw new EncryptionProviderException($"Encryption provider code unknown: {providerCode}")
            {
                Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
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
