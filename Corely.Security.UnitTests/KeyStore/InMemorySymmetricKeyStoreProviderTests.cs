using AutoFixture;
using Corely.Security.KeyStore;

namespace Corely.Security.UnitTests.KeyStore;

public class InMemorySymmetricKeyStoreProviderTests
{
    private readonly Fixture _fixture = new();

    [Fact]
    public void GetCurrentKey_ReturnsKey()
    {
        var key = _fixture.Create<string>();
        var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

        var currentKey = keyStoreProvider.GetCurrentKey();

        Assert.Equal(key, currentKey);
    }

    [Fact]
    public void GetCurrentVersion_ReturnsOne()
    {
        var key = _fixture.Create<string>();
        var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

        var currentVersion = keyStoreProvider.GetCurrentVersion();

        Assert.Equal(1, currentVersion);
    }

    [Fact]
    public void Add_IncrementsVersion()
    {
        var key = _fixture.Create<string>();
        var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

        keyStoreProvider.Add(key);

        var currentKey = keyStoreProvider.GetCurrentKey();
        var currentVersion = keyStoreProvider.GetCurrentVersion();

        Assert.Equal(key, currentKey);
        Assert.Equal(2, currentVersion);
    }

    [Fact]
    public void Get_ReturnsKey()
    {
        var key = _fixture.Create<string>();
        var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(_fixture.Create<string>());

        keyStoreProvider.Add(key);
        keyStoreProvider.Add(_fixture.Create<string>());

        var keyForVersion = keyStoreProvider.Get(2);

        Assert.Equal(key, keyForVersion);
    }

    [Fact]
    public void Get_Throws_WhenVersionIsInvalid()
    {
        var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(_fixture.Create<string>());

        var ex = Record.Exception(() => keyStoreProvider.Get(2));

        Assert.NotNull(ex);
        Assert.IsType<KeyStoreException>(ex);
    }
}
