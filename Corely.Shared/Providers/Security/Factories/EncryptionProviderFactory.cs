using Corely.Shared.Extensions;
using Corely.Shared.Providers.Security.Encryption;
using Corely.Shared.Providers.Security.Exceptions;
using Corely.Shared.Providers.Security.Secrets;

namespace Corely.Shared.Providers.Security.Factories
{
    public class EncryptionProviderFactory : IEncryptionProviderFactory
    {
        protected readonly ISecretProvider _secretProvider;

        public EncryptionProviderFactory(ISecretProvider secretProvider)
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
