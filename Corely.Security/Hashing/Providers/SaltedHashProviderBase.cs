using System.Security.Cryptography;
using System.Text;

namespace Corely.Security.Hashing.Providers;

public abstract class SaltedHashProviderBase : IHashProvider
{
    private const int SALT_SIZE = 16;

    public abstract string HashTypeCode { get; }

    public SaltedHashProviderBase()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(HashTypeCode, nameof(HashTypeCode));
        if (HashTypeCode.Contains(':'))
        {
            throw new HashException($"Hash type code cannot contain ':'")
            {
                Reason = HashException.ErrorReason.InvalidTypeCode
            };
        }
    }

    public string Hash(string value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var salt = CreateSalt();
        var saltedValue = CreateSaltedValue(salt, value);
        var hashedValue = HashInternal(saltedValue);
        return FormatHashedValue(salt, hashedValue);
    }

    private static byte[] CreateSalt()
    {
        byte[] salt = new byte[SALT_SIZE];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    private static byte[] CreateSaltedValue(byte[] salt, string value)
    {
        return [.. salt, .. Encoding.UTF8.GetBytes(value)];
    }

    private string FormatHashedValue(byte[] salt, byte[] hash)
    {
        var saltedHash = salt.Concat(hash).ToArray();
        var finalHash = Convert.ToBase64String(saltedHash);
        return $"{HashTypeCode}:{finalHash}";
    }

    public virtual bool Verify(string value, string originalHash)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        ArgumentNullException.ThrowIfNull(originalHash, nameof(originalHash));

        byte[] salt;
        try
        {
            salt = ValidateForSalt(originalHash);
        }
        catch (HashException)
        {
            return false;
        }

        var saltedValue = CreateSaltedValue(salt, value);
        var hashedValue = HashInternal(saltedValue);
        var finalHash = FormatHashedValue(salt, hashedValue);

        return finalHash == originalHash;
    }

    private byte[] ValidateForSalt(string hash)
    {
        if (!hash.StartsWith(HashTypeCode))
        {
            throw new HashException($"Hash must start with {HashTypeCode}")
            {
                Reason = HashException.ErrorReason.InvalidFormat
            };
        }

        string[] parts = hash.Split(':');
        if (parts.Length != 2
            || string.IsNullOrWhiteSpace(parts[1]))
        {
            throw new HashException($"Hash must be in format hashTypeCode:hashedValue")
            {
                Reason = HashException.ErrorReason.InvalidFormat
            };
        }

        byte[] hashBytes = Convert.FromBase64String(parts[1]);
        if (hashBytes.Length < SALT_SIZE)
        {
            throw new HashException($"Hashed value must be at least {SALT_SIZE} bytes")
            {
                Reason = HashException.ErrorReason.InvalidFormat
            };
        }

        return hashBytes[..SALT_SIZE];
    }

    protected abstract byte[] HashInternal(byte[] value);
}
