using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;

namespace Corely.Security.UnitTests.Encryption.Providers;

public class AesEncryptionProviderTests : SymmetricEncryptionProviderGenericTests
{
    private readonly AesEncryptionProvider _aesEncryptionProvider = new();

    [Fact]
    public override void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation()
    {
        Assert.Equal(SymmetricEncryptionConstants.AES_CODE, _aesEncryptionProvider.EncryptionTypeCode);
    }

    [Fact]
    public override void GetSymmetricKeyProvider_ReturnsCorrectKeyProvider_ForImplementation()
    {
        var keyProvider = _aesEncryptionProvider.GetSymmetricKeyProvider();

        Assert.NotNull(keyProvider);
        Assert.IsType<AesKeyProvider>(keyProvider);
    }

    public override ISymmetricEncryptionProvider GetEncryptionProvider()
    {
        return new AesEncryptionProvider();
    }
}
