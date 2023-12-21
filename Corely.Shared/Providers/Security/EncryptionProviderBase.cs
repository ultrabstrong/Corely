using Corely.Shared.Extensions;

namespace Corely.Shared.Providers.Security
{
    public abstract class EncryptionProviderBase : IEncryptionProvider
    {
        private const int TWO_DIGIT_ENCRYPTION_TYPE_CODE_LENGTH = 2;

        protected readonly string _key;

        protected abstract string TwoDigitEncryptionTypeCode { get; }

        public EncryptionProviderBase(ISecretProvider secretProvider)
        {
            _key = secretProvider.ThrowIfNull(nameof(secretProvider)).Get();
            ValidateEncryptionTypeCode();
        }
        private void ValidateEncryptionTypeCode()
        {
            if (!short.TryParse(TwoDigitEncryptionTypeCode, out _)
                || TwoDigitEncryptionTypeCode.Length != TWO_DIGIT_ENCRYPTION_TYPE_CODE_LENGTH)
            {
                throw new ArgumentException($"{nameof(TwoDigitEncryptionTypeCode)} must be two digit characters [0-9][0-9]");
            }
        }

        public string Decrypt(string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value, nameof(value));
            var valueToDecrypt = value[TWO_DIGIT_ENCRYPTION_TYPE_CODE_LENGTH..];
            return DecryptInternal(valueToDecrypt);
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
