using Corely.Shared.Providers.Security;

namespace Corely.UnitTests.Shared.Providers.Security
{
    public class AesEncryptionProviderTests
    {
        private readonly AESEncryptionProvider _aesEncryptionProvider;

        private readonly Mock<IKeyProvider> _keyProvider;

        public AesEncryptionProviderTests()
        {
            _keyProvider = SetupMockKeyProvider();

            _aesEncryptionProvider = new AESEncryptionProvider(
                _keyProvider.Object,
                "asdf");
        }

        private Mock<IKeyProvider> SetupMockKeyProvider()
        {
            var keyProvider = new Mock<IKeyProvider>();

            keyProvider
                .Setup(m => m.GetKey())
                .Returns("12345678901234567890123456789012");

            return keyProvider;
        }
    }
}
