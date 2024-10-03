using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Providers
{
    public abstract class AsymmetricEncryptionProviderBase : IAsymmetricEncryptionProvider
    {
        public abstract string EncryptionTypeCode { get; }

        public AsymmetricEncryptionProviderBase()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(EncryptionTypeCode, nameof(EncryptionTypeCode));

            if (EncryptionTypeCode.Contains(':'))
            {
                throw new EncryptionException($"Asymmetric encryption type code cannot contain ':'")
                {
                    Reason = EncryptionException.ErrorReason.InvalidTypeCode
                };
            }
        }

        public string Encrypt(string value, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            var (publicKey, _) = keyStoreProvider.GetCurrentKeys();
            var encryptedValue = EncryptInternal(value, publicKey);
            var version = keyStoreProvider.GetCurrentVersion();
            return FormatEncryptedValue(encryptedValue, version);
        }

        public string Decrypt(string value, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
            (var encryptedValue, var version) = ValidateForKeyVersion(value);
            var (_, privateKey) = keyStoreProvider.Get(version);
            return DecryptInternal(encryptedValue, privateKey);
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

        public string ReEncrypt(string value, IAsymmetricEncryptionKeyStoreProvider keyStoreProvider)
        {
            (var encryptedValue, var version) = ValidateForKeyVersion(value);

            var (_, privateKey) = keyStoreProvider.Get(version);
            var decrypted = DecryptInternal(encryptedValue, privateKey);

            var (publicKey, _) = keyStoreProvider.GetCurrentKeys();
            var updatedEncryptedValue = EncryptInternal(decrypted, publicKey);

            var currentVersion = keyStoreProvider.GetCurrentVersion();
            return FormatEncryptedValue(updatedEncryptedValue, currentVersion);
        }

        private string FormatEncryptedValue(string encryptedValue, int keyVersion)
        {
            return $"{EncryptionTypeCode}:{keyVersion}:{encryptedValue}";
        }
        public string? RemoveEncodedEncryptionData(string value)
        {
            return value?.Split(':')?.Last();
        }

        protected abstract string DecryptInternal(string value, string privateKey);

        protected abstract string EncryptInternal(string value, string publicKey);
    }
}
