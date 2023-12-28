using Corely.Common.Providers.Security.Hashing;

namespace Corely.UnitTests.Common.Providers.Security.Hashing
{
    public class Sha256SaltedHashProviderTests : SaltedHashProviderGenericTests
    {
        private readonly Sha256SaltedHashProvider _sha256SaltedHashProvider = new();

        protected override IHashProvider HashProvider => _sha256SaltedHashProvider;

        [Fact]
        public override void HashTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(HashProviderConstants.SALTED_SHA256, _sha256SaltedHashProvider.HashTypeCode);
        }
    }
}
