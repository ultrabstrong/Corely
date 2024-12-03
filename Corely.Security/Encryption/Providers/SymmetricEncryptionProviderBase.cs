using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Providers;

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

    public string Encrypt(string value, ISymmetricKeyStoreProvider keyStoreProvider)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        var key = keyStoreProvider.GetCurrentKey();
        var encryptedValue = EncryptInternal(value, key);
        var version = keyStoreProvider.GetCurrentVersion();
        return FormatEncryptedValue(encryptedValue, version);
    }

    public string Decrypt(string value, ISymmetricKeyStoreProvider keyStoreProvider)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        (var encryptedValue, var version) = ValidateForKeyVersion(value);
        var key = keyStoreProvider.Get(version);
        return DecryptInternal(encryptedValue, key);
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

    public string ReEncrypt(string value, ISymmetricKeyStoreProvider keyStoreProvider)
    {
        (var encryptedValue, var version) = ValidateForKeyVersion(value);

        var decryptKey = keyStoreProvider.Get(version);
        var decrypted = DecryptInternal(encryptedValue, decryptKey);

        var encryptKey = keyStoreProvider.GetCurrentKey();
        var updatedEncryptedValue = EncryptInternal(decrypted, encryptKey);

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

    public abstract ISymmetricKeyProvider GetSymmetricKeyProvider();

    protected abstract string DecryptInternal(string value, string key);

    protected abstract string EncryptInternal(string value, string key);
}
