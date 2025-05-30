﻿using AutoFixture;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.Security.UnitTests.Encryption.Models;

public class SymmetricEncryptedValueTests
{
    private readonly InMemorySymmetricKeyStoreProvider _keyStoreProvider;
    private readonly SymmetricEncryptedValue _encryptedValue;
    private readonly Fixture _fixture = new();

    public SymmetricEncryptedValueTests()
    {
        var key = new AesKeyProvider().CreateKey();
        _keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

        var encryptionProvider = new AesEncryptionProvider();
        _encryptedValue = new SymmetricEncryptedValue(encryptionProvider);
    }

    [Fact]
    public void Constructor_CreatesEncryptedValue()
    {
        Assert.NotNull(_encryptedValue);
    }

    [Fact]
    public void Constructor_CreatesEncryptedValueWithSecret()
    {
        var encryptionProvider = new AesEncryptionProvider();
        var value = _fixture.Create<string>();

        var encryptedValue = new SymmetricEncryptedValue(encryptionProvider) { Secret = value };

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
        var oldSecret = _encryptedValue.Secret;
        _encryptedValue.ReEncrypt(_keyStoreProvider);
        Assert.NotEqual(oldSecret, _encryptedValue.Secret);
    }
}
