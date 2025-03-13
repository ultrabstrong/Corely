using Corely.Security.Hashing;
using Corely.Security.Hashing.Providers;

namespace Corely.Security.UnitTests.Hashing.Providers;

public class Sha512SaltedHashProviderTests : SaltedHashProviderGenericTests
{
    private readonly Sha512SaltedHashProvider _sha512SaltedHashProvider = new();

    protected override IHashProvider HashProvider => _sha512SaltedHashProvider;

    [Fact]
    public override void HashTypeCode_ReturnsCorrectCode_ForImplementation()
    {
        Assert.Equal(HashConstants.SALTED_SHA512_CODE, _sha512SaltedHashProvider.HashTypeCode);
    }
}
