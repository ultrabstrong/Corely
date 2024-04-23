using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.Keys;
using Corely.Security.KeyStore;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public class AesEncryptionProviderTests : EncryptionProviderGenericTests
    {
        private readonly AesKeyProvider _keyProvider = new();
        private readonly InMemoryKeyStoreProvider _keyStoreProvider;
        private readonly AesEncryptionProvider _aesEncryptionProvider;

        public AesEncryptionProviderTests()
        {
            _keyStoreProvider = new(_keyProvider.CreateKey());
            _aesEncryptionProvider = new(_keyStoreProvider);
        }

        [Fact]
        public override void EncryptionTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(EncryptionConstants.AES_CODE, _aesEncryptionProvider.EncryptionTypeCode);
        }

        public override IEncryptionProvider GetEncryptionProvider(IKeyStoreProvider keyStoreProvider)
        {
            return new AesEncryptionProvider(keyStoreProvider);
        }
    }
}
