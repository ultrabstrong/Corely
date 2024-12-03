using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore;

namespace Corely.Security.Encryption.Models;

public class SymmetricEncryptedValue : ISymmetricEncryptedValue
{
    private readonly object _lock = new();
    private readonly ISymmetricEncryptionProvider _encryptionProvider;

    public SymmetricEncryptedValue(ISymmetricEncryptionProvider encryptionProvider)
    {
        ArgumentNullException.ThrowIfNull(encryptionProvider, nameof(encryptionProvider));
        _encryptionProvider = encryptionProvider;
    }

    public string Secret
    {
        get => _secret;
        init => _secret = value;
    }
    private string _secret = string.Empty;

    public void Set(string decryptedValue, ISymmetricKeyStoreProvider provider)
    {
        var encryptedValue = _encryptionProvider.Encrypt(decryptedValue, provider);
        lock (_lock)
        {
            _secret = encryptedValue;
        }
    }

    public string GetDecrypted(ISymmetricKeyStoreProvider provider)
    {
        return _encryptionProvider.Decrypt(Secret, provider);
    }

    public void ReEncrypt(ISymmetricKeyStoreProvider provider)
    {
        _secret = _encryptionProvider.ReEncrypt(Secret, provider);
    }
}
