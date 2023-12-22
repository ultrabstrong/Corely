using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Keys;

namespace Corely.Common.Providers.Security.Factories
{
    public class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        protected readonly IKeyStoreProvider _keyStoreProvider;

        public EncryptionProviderFactory(IKeyStoreProvider keyStoreProvider)
        {
            _keyStoreProvider = keyStoreProvider.ThrowIfNull(nameof(keyStoreProvider));
        }

        public IEncryptionProvider Create(string providerCode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(providerCode, nameof(providerCode));

            return providerCode switch
            {
                EncryptionProviderConstants.AES => new AesEncryptionProvider(_keyStoreProvider),
                _ => throw new EncryptionProviderException($"Unknown encryption provider code {providerCode}")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
                }
            };
        }

        public IEncryptionProvider CreateForDecrypting(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            string[] parts = value.Split(':');
            if (parts.Length < 1)
            {
                throw new EncryptionProviderException("Value must be in format encryptionTypeCode:integer:encryptedValue")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidFormat
                };
            }

            return Create(parts[0]);
        }
    }
}
