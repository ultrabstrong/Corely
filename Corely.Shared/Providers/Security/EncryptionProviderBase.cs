using Corely.Shared.Extensions;

namespace Corely.Shared.Providers.Security
{
    public abstract class EncryptionProviderBase : IEncryptionProvider
    {
        protected readonly string Key;

        protected abstract string TwoDigitEncryptionTypeCode { get; }

        public EncryptionProviderBase(ISecretProvider secretProvider)
        {
            Key = secretProvider.ThrowIfNull(nameof(secretProvider)).Get();
            ValidateEncryptionTypeCode(TwoDigitEncryptionTypeCode);
        }

        public string Decrypt(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));

            var encryptionTypeCode = value[..EncryptionProviderCodeConstants.ENCRYPTION_TYPE_CODE_LENGTH];
            ValidateEncryptionTypeCode(encryptionTypeCode);

            var valueToDecrypt = value[EncryptionProviderCodeConstants.ENCRYPTION_TYPE_CODE_LENGTH..];
            return DecryptInternal(valueToDecrypt);
        }

        private void ValidateEncryptionTypeCode(string encryptionTypeCode)
        {
            if (!short.TryParse(encryptionTypeCode, out _)
                || encryptionTypeCode.Length != EncryptionProviderCodeConstants.ENCRYPTION_TYPE_CODE_LENGTH)
            {
                throw new ArgumentException($"{nameof(encryptionTypeCode)} must be two digit characters [0-9][0-9]");
            }
        }

        public string Encrypt(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
            var encryptedValue = EncryptInternal(value);
            return $"{TwoDigitEncryptionTypeCode}{encryptedValue}";
        }

        protected abstract string DecryptInternal(string value);
        protected abstract string EncryptInternal(string value);
    }
}
