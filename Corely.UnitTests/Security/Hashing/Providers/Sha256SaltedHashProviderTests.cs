using Corely.Security.Hashing;
using Corely.Security.Hashing.Providers;

namespace Corely.UnitTests.Security.Hashing.Providers
{
    public class Sha256SaltedHashProviderTests : SaltedHashProviderGenericTests
    {
        private readonly Sha256SaltedHashProvider _sha256SaltedHashProvider = new();

        protected override IHashProvider HashProvider => _sha256SaltedHashProvider;

        [Fact]
        public override void HashTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(HashConstants.SALTED_SHA256_CODE, _sha256SaltedHashProvider.HashTypeCode);
        }
    }
}
