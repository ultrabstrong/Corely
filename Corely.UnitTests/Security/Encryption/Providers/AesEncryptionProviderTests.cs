using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public class AesEncryptionProviderTests : SymmetricEncryptionProviderGenericTests
    {
        private readonly AesEncryptionProvider _aesEncryptionProvider = new();

        public override void EncryptionTypeCode_ReturnsCorrectCode_ForImplementation()
        {
            Assert.Equal(SymmetricEncryptionConstants.AES_CODE, _aesEncryptionProvider.EncryptionTypeCode);
        }

        public override ISymmetricEncryptionProvider GetEncryptionProvider()
        {
            return new AesEncryptionProvider();
        }
    }
}
