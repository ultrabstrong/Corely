using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Keys;

namespace Corely.Common.Providers.Security.Factories
{
    public class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        protected readonly IKeyStoreProvider _secretProvider;

        public EncryptionProviderFactory(IKeyStoreProvider secretProvider)
        {
            _secretProvider = secretProvider.ThrowIfNull(nameof(secretProvider));
        }

        public IEncryptionProvider Create() => new AesEncryptionProvider(_secretProvider);

        public IEncryptionProvider CreateForDecrypting(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));

            if (value.Length < 2)
            {
                throw GetProviderCodeException(value);
            }

            string providerCode = value[..2];

            return providerCode switch
            {
                EncryptionProviderConstants.Aes => new AesEncryptionProvider(_secretProvider),
                _ => throw GetProviderCodeException(providerCode)
            };
        }

        private EncryptionProviderException GetProviderCodeException(string providerCode)
        {
            return new EncryptionProviderException($"Unknown encryption provider code {providerCode}")
            {
                Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
            };
        }
    }
}
