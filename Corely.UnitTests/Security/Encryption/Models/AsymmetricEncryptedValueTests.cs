using AutoFixture;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;
using Corely.Security.KeyStore;
using System.Security.Cryptography;

namespace Corely.UnitTests.Security.Encryption.Models;

public class AsymmetricEncryptedValueTests
{
    private readonly InMemoryAsymmetricKeyStoreProvider _keyStoreProvider;
    private readonly AsymmetricEncryptedValue _encryptedValue;
    private readonly Fixture _fixture = new();

    public AsymmetricEncryptedValueTests()
    {
        var (publicKey, privateKey) = new RsaKeyProvider().CreateKeys();
        _keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);

        var encryptionProvider = new RsaEncryptionProvider(RSAEncryptionPadding.OaepSHA256);
        _encryptedValue = new AsymmetricEncryptedValue(encryptionProvider);
    }

    [Fact]
    public void Constructor_CreatesEncryptedValue()
    {
        Assert.NotNull(_encryptedValue);
    }

    [Fact]
    public void Constructor_CreatesEncryptedValueWithSecret()
    {
        var encryptionProvider = new RsaEncryptionProvider(RSAEncryptionPadding.OaepSHA256);
        var value = _fixture.Create<string>();

        var encryptedValue = new AsymmetricEncryptedValue(encryptionProvider) { Secret = value };

        Assert.Equal(value, encryptedValue.Secret);
    }

    [Fact]
    public void Set_SetsEncryptedSecret()
    {
        var value = _fixture.Create<string>();
        _encryptedValue.Set(value, _keyStoreProvider);
        Assert.NotNull(_encryptedValue.Secret);
        Assert.NotEmpty(_encryptedValue.Secret);
        Assert.NotEqual(value, _encryptedValue.Secret);
    }

    [Fact]
    public void Get_GetsDecryptedSecret()
    {
        var value = _fixture.Create<string>();
        _encryptedValue.Set(value, _keyStoreProvider);
        var decryptedValue = _encryptedValue.GetDecrypted(_keyStoreProvider);
        Assert.Equal(value, decryptedValue);
    }

    [Fact]
    public void ReEncrypt_ReEncryptsSecret()
    {
        var value = _fixture.Create<string>();
        _encryptedValue.Set(value, _keyStoreProvider);
        var encryptedValue = _encryptedValue.Secret;
        _encryptedValue.ReEncrypt(_keyStoreProvider);
        Assert.NotEqual(encryptedValue, _encryptedValue.Secret);
    }
}
