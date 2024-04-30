using Corely.Security.KeyStore.Symmetric;

namespace Corely.Security.Encryption.Providers
{
    public abstract class SymmetricEncryptionProviderBase : ISymmetricEncryptionProvider
    {
        public abstract string EncryptionTypeCode { get; }

        public SymmetricEncryptionProviderBase()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(EncryptionTypeCode, nameof(EncryptionTypeCode));

            if (EncryptionTypeCode.Contains(':'))
            {
                throw new EncryptionException($"Symmetric encryption type code cannot contain ':'")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public string Encrypt(string value, ISymmetricKeyStoreProvider provider)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            (var key, var version) = provider.GetCurrentVersion();
            var encryptedValue = EncryptInternal(value, key);
            return FormatEncryptedValue(encryptedValue, version);
        }

        public string Decrypt(string value, ISymmetricKeyStoreProvider provider)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            return DecryptInternal(encryptedValue, provider.Get(version));
        }

        private (string, int) ValidateForKeyVersion(string value)
        {
            if (!value.StartsWith(EncryptionTypeCode))
            {
                throw new EncryptionException($"Value must start with {EncryptionTypeCode}")
                {
                    Reason = EncryptionException.ErrorReason.InvalidFormat
                };
            }

            string[] parts = value.Split(':');

            if (parts.Length != 3
                || string.IsNullOrWhiteSpace(parts[2])
                || !int.TryParse(parts[1], out var keyVersion))
            {
                throw new EncryptionException("Value must be in format encryptionTypeCode:integer:encryptedValue")
                {
                    Reason = EncryptionException.ErrorReason.InvalidFormat
                };
            }

            return (parts[2], keyVersion);
        }

        public string ReEncrypt(string value, ISymmetricKeyStoreProvider provider)
        {
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            (var key, var currentVersion) = provider.GetCurrentVersion();

            var decrypted = DecryptInternal(encryptedValue, provider.Get(version));
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
