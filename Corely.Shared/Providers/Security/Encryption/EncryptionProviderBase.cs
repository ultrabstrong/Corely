using Corely.Shared.Extensions;
using Corely.Shared.Providers.Security.Exceptions;
using Corely.Shared.Providers.Security.Keys;
using System.Text.RegularExpressions;

namespace Corely.Shared.Providers.Security.Encryption
{
    public abstract class EncryptionProviderBase : IEncryptionProvider
    {
        protected readonly IKeyStoreProvider _secretProvider;

        protected abstract string TwoDigitEncryptionTypeCode { get; }

        public EncryptionProviderBase(IKeyStoreProvider secretProvider)
        {
            _secretProvider = secretProvider.ThrowIfNull(nameof(secretProvider));
            TwoDigitEncryptionTypeCode.ThrowIfNullOrWhiteSpace(nameof(TwoDigitEncryptionTypeCode));

            Regex reg = new(EncryptionProviderConstants.ENCRYPTION_TYPE_CODE_REGEX);
            if (!reg.IsMatch(TwoDigitEncryptionTypeCode))
            {
                throw new EncryptionProviderException($"{nameof(TwoDigitEncryptionTypeCode)} must be two digit characters [0-9][0-9]")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public string Encrypt(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            (var key, var version) = _secretProvider.GetCurrentVersion();
            var encryptedValue = EncryptInternal(value, key);
            return FormatEncryptedValue(encryptedValue, version);
        }

        public string Decrypt(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            return DecryptInternal(encryptedValue, _secretProvider.Get(version));
        }

        private (string, int) ValidateForKeyVersion(string value)
        {
            if (!value.StartsWith(TwoDigitEncryptionTypeCode))
            {
                throw new EncryptionProviderException($"Value must start with {TwoDigitEncryptionTypeCode}")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidFormat
                };
            }

            string[] parts = value.Split(':');

            if (parts.Length != 2
                || parts[0].Length < 3
                || string.IsNullOrWhiteSpace(parts[1])
                || !int.TryParse(parts[0][TwoDigitEncryptionTypeCode.Length..], out var keyVersion))
            {
                throw new EncryptionProviderException($"Value must be in format [0-9][0-9]integer:encryptedValue")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidFormat
                };
            }

            return (parts[1], keyVersion);
        }

        public string ReEncryptWithCurrentKey(string value, bool skipIfAlreadyCurrent = true)
        {
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            (var key, var currentVersion) = _secretProvider.GetCurrentVersion();

            if (skipIfAlreadyCurrent && version == currentVersion)
            {
                return value;
            }

            var decrypted = DecryptInternal(encryptedValue, _secretProvider.Get(version));
            var updatedEncryptedValue = EncryptInternal(decrypted, key);
            return FormatEncryptedValue(updatedEncryptedValue, currentVersion);
        }

        private string FormatEncryptedValue(string encryptedValue, int keyVersion)
        {
            return $"{TwoDigitEncryptionTypeCode}{keyVersion}:{encryptedValue}";
        }

        protected abstract string DecryptInternal(string value, string key);

        protected abstract string EncryptInternal(string value, string key);
    }
}
