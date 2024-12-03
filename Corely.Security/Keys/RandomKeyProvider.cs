using System.Security.Cryptography;

namespace Corely.Security.Keys;

internal sealed class RandomKeyProvider : ISymmetricKeyProvider
{
    public const int DEFAULT_KEY_SIZE = 32;

    private readonly int _keySize;

    public RandomKeyProvider(int keySize = DEFAULT_KEY_SIZE)
    {
        if (keySize < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(keySize), "Key size must be a positive number.");
        }
        _keySize = keySize;
    }

    public string CreateKey()
    {
        var keyBytes = new byte[_keySize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(keyBytes);
        }

        return Convert.ToBase64String(keyBytes);
    }

    public bool IsKeyValid(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        var keyBytes = Convert.FromBase64String(key);
        return keyBytes.Length == _keySize;
    }
}
