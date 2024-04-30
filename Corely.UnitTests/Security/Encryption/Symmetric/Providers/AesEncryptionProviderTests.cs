using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys.Symmetric;
using Corely.Security.KeyStore.Symmetric;

namespace Corely.UnitTests.Security.Encryption.Symmetric.Providers
{
    public class AesEncryptionProviderTests : SymmetricEncryptionProviderGenericTests
    {
        private readonly AesKeyProvider _keyProvider = new();
        private readonly InMemorySymmetricKeyStoreProvider _keyStoreProvider;
        private readonly AesEncryptionProvider _aesEncryptionProvider;

        public AesEncryptionProviderTests()
        {
            _keyStoreProvider = new(_keyProvider.CreateKey());
            _aesEncryptionProvider = new(_keyStoreProvider);
        }

        [Fact]
        public override void EncryptionTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(SymmetricEncryptionConstants.AES_CODE, _aesEncryptionProvider.EncryptionTypeCode);
        }

        public override ISymmetricEncryptionProvider GetEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
        {
            return new AesEncryptionProvider(keyStoreProvider);
        }
    }
}
