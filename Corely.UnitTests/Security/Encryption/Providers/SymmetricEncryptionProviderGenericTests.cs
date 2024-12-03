using AutoFixture;
using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;
using Corely.Security.KeyStore;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Security.Encryption.Providers;

public abstract class SymmetricEncryptionProviderGenericTests
{
    private readonly Fixture _fixture = new();
    private readonly AesKeyProvider _keyProvider;
    private readonly InMemorySymmetricKeyStoreProvider _keyStoreProvider;
    private readonly ISymmetricEncryptionProvider _encryptionProvider;

    public SymmetricEncryptionProviderGenericTests()
    {
        _keyProvider = new AesKeyProvider();
        _keyStoreProvider = new InMemorySymmetricKeyStoreProvider(_keyProvider.CreateKey());
        _encryptionProvider = GetEncryptionProvider();
    }

    [Fact]
    public void Encrypt_ReturnsCorrectlyFormattedValue()
    {
        var decrypted = _fixture.Create<string>();

        var encrypted = _encryptionProvider.Encrypt(decrypted, _keyStoreProvider);

        Assert.StartsWith(_encryptionProvider.EncryptionTypeCode, encrypted);
        Assert.Matches(@"^.+:\d+:.+", encrypted);
        Assert.NotEqual(decrypted, encrypted);
    }

    [Theory, ClassData(typeof(EmptyAndWhitespace))]
    public void Encrypt_ReturnsCorrectlyFormattedValue_WithEmptyAndWhitespace(string value)
    {
        var encrypted = _encryptionProvider.Encrypt(value, _keyStoreProvider);
        Assert.StartsWith(_encryptionProvider.EncryptionTypeCode, encrypted);
        Assert.Matches(@"^.+:\d+:.+", encrypted);
        Assert.NotEqual(value, encrypted);
    }

    [Fact]
    public void Encrypt_ProducesDifferentEncryptedStrings()
    {
        var decrypted = _fixture.Create<string>();
        var encrypted1 = _encryptionProvider.Encrypt(decrypted, _keyStoreProvider);
        var encrypted2 = _encryptionProvider.Encrypt(decrypted, _keyStoreProvider);
        Assert.NotEqual(encrypted1, encrypted2);
    }

    [Fact]
    public void Encrypt_Throws_WithNullInput()
    {
        var ex = Record.Exception(() => _encryptionProvider.Encrypt(null!, _keyStoreProvider));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void Encrypt_ThenDecrypt_ProducesOriginalValue()
    {
        var originalDecrypted = _fixture.Create<string>();
        var encrypted = _encryptionProvider.Encrypt(originalDecrypted, _keyStoreProvider);
        var decrypted = _encryptionProvider.Decrypt(encrypted, _keyStoreProvider);
        Assert.Equal(originalDecrypted, decrypted);
    }

    [Fact]
    public void Decrypt_ProducesSameStringThatWasEncrypted()
    {
        var decrpyted = _fixture.Create<string>();
        var encrypted1 = _encryptionProvider.Encrypt(decrpyted, _keyStoreProvider);
        var encrypted2 = _encryptionProvider.Encrypt(decrpyted, _keyStoreProvider);
        var decrypted1 = _encryptionProvider.Decrypt(encrypted1, _keyStoreProvider);
        var decrypted2 = _encryptionProvider.Decrypt(encrypted2, _keyStoreProvider);
        Assert.Equal(decrpyted, decrypted1);
        Assert.Equal(decrpyted, decrypted2);
    }

    [Fact]
    public void Decrypt_Succeeds_AfterKeyIsUpdated()
    {
        var originalDecrypted = _fixture.Create<string>();
        var encrypted = _encryptionProvider.Encrypt(originalDecrypted, _keyStoreProvider);

        _keyStoreProvider.Add(_keyProvider.CreateKey());

        var decrypted = _encryptionProvider.Decrypt(encrypted, _keyStoreProvider);
        Assert.Equal(originalDecrypted, decrypted);
    }

    [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
    public void Decrypt_Throws_WithNullOrWhiteSpace(string value)
    {
        var ex = Record.Exception(() => _encryptionProvider.Decrypt(value, _keyStoreProvider));
        Assert.NotNull(ex);
        Assert.True(ex is ArgumentNullException || ex is ArgumentException);
    }

    [Theory]
    [InlineData("--", true)]
    [InlineData("--:", false)]
    [InlineData("--::", false)]
    [InlineData(":", false)]
    [InlineData("::", false)]
    [InlineData("", true)]
    [InlineData(":", true)]
    [InlineData("::", true)]
    [InlineData(":1", true)]
    [InlineData(":2:", true)]
    public void Decrypt_Throws_WithInvalidFormat(string value, bool prependTypeCode)
    {
        var testValue = prependTypeCode
            ? $"{_encryptionProvider.EncryptionTypeCode}{value}"
            : value;

        var ex = Record.Exception(() => _encryptionProvider.Decrypt(testValue, _keyStoreProvider));

        Assert.NotNull(ex);
        Assert.IsType<EncryptionException>(ex);
    }

    [Fact]
    public void ReEncrypt_ReturnsUpdatedCurrentVersionValue_WhenKeyVersionIsSame()
    {
        var originalDecrypted = _fixture.Create<string>();
        var originalEncrypted = _encryptionProvider.Encrypt(originalDecrypted, _keyStoreProvider);

        var encrypted = _encryptionProvider.ReEncrypt(originalEncrypted, _keyStoreProvider);
        var decrypted = _encryptionProvider.Decrypt(encrypted, _keyStoreProvider);

        Assert.Equal(originalDecrypted, decrypted);
        Assert.NotEqual(originalEncrypted, encrypted);
        Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:1:", originalEncrypted);
        Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:1:", encrypted);
    }

    [Fact]
    public void ReEncrypt_ReturnsUpdatedNewVersionValue_WhenKeyVersionIsChanged()
    {
        var originalDecrypted = _fixture.Create<string>();
        var originalEncrypted = _encryptionProvider.Encrypt(originalDecrypted, _keyStoreProvider);

        _keyStoreProvider.Add(_keyProvider.CreateKey());

        var encrypted = _encryptionProvider.ReEncrypt(originalEncrypted, _keyStoreProvider);
        var decrypted = _encryptionProvider.Decrypt(encrypted, _keyStoreProvider);

        Assert.Equal(originalDecrypted, decrypted);
        Assert.NotEqual(originalEncrypted, encrypted);
        Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:1:", originalEncrypted);
        Assert.StartsWith($"{_encryptionProvider.EncryptionTypeCode}:2:", encrypted);
    }

    [Fact]
    public void RemoveEncodedEncryptionData_ReturnsOnlyEncryptedValue()
    {
        var decrypted = _fixture.Create<string>();
        var encrypted = _encryptionProvider.Encrypt(decrypted, _keyStoreProvider);

        var noEncoding = _encryptionProvider.RemoveEncodedEncryptionData(encrypted);

        Assert.DoesNotContain(':', noEncoding!);
        Assert.EndsWith(noEncoding, encrypted);
    }

    [Fact]
    public void RemoveEncodedEncryptionData_ReturnsNull_WithNullInput()
    {
        Assert.Null(_encryptionProvider.RemoveEncodedEncryptionData(null!));
    }

    [Fact]
    public abstract void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation();

    [Fact]
    public abstract void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation();

    public abstract ISymmetricEncryptionProvider GetEncryptionProvider();

}
