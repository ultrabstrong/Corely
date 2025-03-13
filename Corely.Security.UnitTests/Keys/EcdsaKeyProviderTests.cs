using Corely.Security.Keys;

namespace Corely.Security.UnitTests.Keys;

public class EcdsaKeyProviderTests
{
    private readonly EcdsaKeyProvider _ecdsaKeyProvider = new();

    [Fact]
    public void CreateKeys_ReturnsValidKeys()
    {
        var (publicKey, privateKey) = _ecdsaKeyProvider.CreateKeys();
        Assert.NotNull(publicKey);
        Assert.NotNull(privateKey);
    }

    [Fact]
    public void IsKeyValid_ReturnsTrue_ForValidKeys()
    {
        var (publicKey, privateKey) = _ecdsaKeyProvider.CreateKeys();
        var isValid = _ecdsaKeyProvider.IsKeyValid(publicKey, privateKey);
        Assert.True(isValid);
    }

    [Fact]
    public void IsKeyValid_ReturnsFalse_ForInvalidPrivateKey()
    {
        var (publicKey, _) = _ecdsaKeyProvider.CreateKeys();
        var invalidKey = Convert.ToBase64String(new byte[256]);
        var isValid = _ecdsaKeyProvider.IsKeyValid(publicKey, invalidKey);
        Assert.False(isValid);
    }

    [Fact]
    public void IsKeyValid_ReturnsFalse_ForInvalidPublicKey()
    {
        _ecdsaKeyProvider.CreateKeys();
        var invalidKey = Convert.ToBase64String(new byte[256]);
        var isValid = _ecdsaKeyProvider.IsKeyValid(invalidKey, invalidKey);
        Assert.False(isValid);
    }
}
