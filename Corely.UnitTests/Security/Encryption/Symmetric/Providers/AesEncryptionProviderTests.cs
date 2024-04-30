using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;

namespace Corely.UnitTests.Security.Encryption.Symmetric.Providers
{
    public class AesEncryptionProviderTests : SymmetricEncryptionProviderGenericTests
    {
        private readonly AesEncryptionProvider _aesEncryptionProvider = new();

        [Fact]
        public override void EncryptionTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(SymmetricEncryptionConstants.AES_CODE, _aesEncryptionProvider.EncryptionTypeCode);
        }

        public override ISymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new AesEncryptionProvider();
        }
    }
}
