using AutoFixture;
using Corely.Security.KeyStore;

namespace Corely.Security.UnitTests.KeyStore;

public class InMemoryAsymmetricKeyStoreProviderTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void GetCurrentKeys_ReturnsKeys()
    {
        var expectedPublicKey = _fixture.Create<string>();
        var expectedPrivateKey = _fixture.Create<string>();
        var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(expectedPublicKey, expectedPrivateKey);

        var (publicKey, privateKey) = keyStoreProvider.GetCurrentKeys();

        Assert.Equal(expectedPublicKey, publicKey);
        Assert.Equal(expectedPrivateKey, privateKey);
    }

    [Fact]
    public void GetCurrentVersion_ReturnsOne()
    {
        var publicKey = _fixture.Create<string>();
        var privateKey = _fixture.Create<string>();
        var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);

        var currentVersion = keyStoreProvider.GetCurrentVersion();

        Assert.Equal(1, currentVersion);
    }

    [Fact]
    public void Add_IncrementsVersion()
    {
        var expectedPublicKey = _fixture.Create<string>();
        var expectedPrivateKey = _fixture.Create<string>();
        var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(expectedPublicKey, expectedPrivateKey);

        keyStoreProvider.Add(expectedPublicKey, expectedPrivateKey);

        var (publicKey, privateKey) = keyStoreProvider.GetCurrentKeys();
        var currentVersion = keyStoreProvider.GetCurrentVersion();

        Assert.Equal(expectedPublicKey, publicKey);
        Assert.Equal(expectedPrivateKey, privateKey);
        Assert.Equal(2, currentVersion);
    }

    [Fact]
    public void Get_ReturnsKey()
    {
        var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(
            _fixture.Create<string>(),
            _fixture.Create<string>());

        var expectedPublicKey = _fixture.Create<string>();
        var expectedPrivateKey = _fixture.Create<string>();

        keyStoreProvider.Add(expectedPublicKey, expectedPrivateKey);
        keyStoreProvider.Add(_fixture.Create<string>(), _fixture.Create<string>());

        var (publicKey, privateKey) = keyStoreProvider.Get(2);

        Assert.Equal(expectedPublicKey, publicKey);
        Assert.Equal(expectedPrivateKey, privateKey);
    }

    [Fact]
    public void Get_Throws_WhenVersionIsInvalid()
    {
        var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(
            _fixture.Create<string>(),
            _fixture.Create<string>());

        var ex = Record.Exception(() => keyStoreProvider.Get(2));

        Assert.NotNull(ex);
        Assert.IsType<KeyStoreException>(ex);
    }
}
