using Corely.Common.Extensions;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Keys;

namespace Corely.Common.Providers.Security.Encryption
{
    public abstract class EncryptionProviderBase : IEncryptionProvider
    {
        private readonly IKeyStoreProvider _keyStoreProvider;

        public abstract string EncryptionTypeCode { get; }

        public EncryptionProviderBase(IKeyStoreProvider keyStoreProvider)
        {
            _keyStoreProvider = keyStoreProvider.ThrowIfNull(nameof(keyStoreProvider));
            EncryptionTypeCode.ThrowIfNullOrWhiteSpace(nameof(EncryptionTypeCode));
            if (EncryptionTypeCode.Contains(':'))
            {
                throw new EncryptionProviderException($"Encryption type code cannot contain ':'")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public string Encrypt(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            (var key, var version) = _keyStoreProvider.GetCurrentVersion();
            var encryptedValue = EncryptInternal(value, key);
            return FormatEncryptedValue(encryptedValue, version);
        }

        public string Decrypt(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            return DecryptInternal(encryptedValue, _keyStoreProvider.Get(version));
        }

        private (string, int) ValidateForKeyVersion(string value)
        {
            if (!value.StartsWith(EncryptionTypeCode))
            {
                throw new EncryptionProviderException($"Value must start with {EncryptionTypeCode}")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidFormat
                };
            }

            string[] parts = value.Split(':');

            if (parts.Length != 3
                || string.IsNullOrWhiteSpace(parts[2])
                || !int.TryParse(parts[1], out var keyVersion))
            {
                throw new EncryptionProviderException("Value must be in format encryptionTypeCode:integer:encryptedValue")
                {
                    Reason = EncryptionProviderException.ErrorReason.InvalidFormat
                };
            }

            return (parts[2], keyVersion);
        }

        public string ReEncrypt(string value)
        {
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            (var key, var currentVersion) = _keyStoreProvider.GetCurrentVersion();

            var decrypted = DecryptInternal(encryptedValue, _keyStoreProvider.Get(version));
            var updatedEncryptedValue = EncryptInternal(decrypted, key);
            return FormatEncryptedValue(updatedEncryptedValue, currentVersion);
        }

        private string FormatEncryptedValue(string encryptedValue, int keyVersion)
        {
            return $"{EncryptionTypeCode}:{keyVersion}:{encryptedValue}";
        }

        protected abstract string DecryptInternal(string value, string key);

        protected abstract string EncryptInternal(string value, string key);
    }
}
