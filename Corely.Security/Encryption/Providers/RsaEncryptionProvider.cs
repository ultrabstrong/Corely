using Corely.Security.Keys;
using System.Security.Cryptography;
using System.Text;

namespace Corely.Security.Encryption.Providers;

public sealed class RsaEncryptionProvider : AsymmetricEncryptionProviderBase
{
    public override string EncryptionTypeCode => AsymmetricEncryptionConstants.RSA_CODE;

    private readonly RsaKeyProvider _rsaKeyProvider = new();
    private readonly RSAEncryptionPadding _rsaEncryptionPadding;

    public RsaEncryptionProvider(RSAEncryptionPadding rsaEncryptionPadding)
    {
        _rsaEncryptionPadding = rsaEncryptionPadding;
    }

    protected override string DecryptInternal(string value, string privateKey)
    {
        var privateKeyBytes = Convert.FromBase64String(privateKey);
        var encryptedBytes = Convert.FromBase64String(value);

        using (var rsa = RSA.Create())
        {
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            var decryptedBytes = rsa.Decrypt(encryptedBytes, _rsaEncryptionPadding);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    protected override string EncryptInternal(string value, string publicKey)
    {
        var publicKeyBytes = Convert.FromBase64String(publicKey);
        var dataToEncrypt = Encoding.UTF8.GetBytes(value);

        using (var rsa = RSA.Create())
        {
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
            var encryptedBytes = rsa.Encrypt(dataToEncrypt, _rsaEncryptionPadding);
            return Convert.ToBase64String(encryptedBytes);
        }
    }

    public override IAsymmetricKeyProvider GetAsymmetricKeyProvider() => _rsaKeyProvider;
}
