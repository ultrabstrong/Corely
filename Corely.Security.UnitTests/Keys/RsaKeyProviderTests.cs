using Corely.Security.Keys;
using System.Security.Cryptography;

namespace Corely.Security.UnitTests.Keys;

public class RsaKeyProviderTests
{
    private readonly RsaKeyProvider _rsaKeyProvider = new();

    [Fact]
    public void Constructor_UsesDefaultKeySize()
    {
        var rsaKeyProvider = new RsaKeyProvider();
        var (publicKey, _) = rsaKeyProvider.CreateKeys();

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
            Assert.Equal(RsaKeyProvider.DEFAULT_KEY_SIZE, rsa.KeySize);
        }
    }

    [Theory]
    [InlineData(123)]
    [InlineData(-2048)]
    public void Constructor_Throws_WithInvalidKeySize(int keySize)
    {
        var ex = Record.Exception(() => new RsaKeyProvider(keySize));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentOutOfRangeException>(ex);
    }

    [Fact]
    public void CreateKeys_ReturnsValidKeys()
    {
        var (publicKey, privateKey) = _rsaKeyProvider.CreateKeys();
        Assert.NotEmpty(publicKey);
        Assert.NotEmpty(privateKey);
    }

    [Fact]
    public void IsKeyValid_ReturnsTrue_ForValidKeys()
    {
        var (publicKey, privateKey) = _rsaKeyProvider.CreateKeys();
        var isValid = _rsaKeyProvider.IsKeyValid(publicKey, privateKey);
        Assert.True(isValid);
    }

    [Fact]
    public void IsKeyValid_ReturnsFalse_ForInvalidPrivateKey()
    {
        var (publicKey, _) = _rsaKeyProvider.CreateKeys();
        var invalidKey = Convert.ToBase64String(new byte[256]);
        var isValid = _rsaKeyProvider.IsKeyValid(publicKey, invalidKey);
        Assert.False(isValid);
    }

    [Fact]
    public void IsKeyValid_ReturnsFalse_ForInvalidPublicKey()
    {
        _rsaKeyProvider.CreateKeys();
        var invalidKey = Convert.ToBase64String(new byte[256]);
        var isValid = _rsaKeyProvider.IsKeyValid(invalidKey, invalidKey);
        Assert.False(isValid);
    }

    [Fact]
    public void CreateKeyPair_RespectsSpecifiedKeySize()
    {
        var rsaKeyProvider = new RsaKeyProvider(4096);
        var (publicKey, _) = rsaKeyProvider.CreateKeys();

        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
            Assert.Equal(4096, rsa.KeySize);
        }
    }
}
