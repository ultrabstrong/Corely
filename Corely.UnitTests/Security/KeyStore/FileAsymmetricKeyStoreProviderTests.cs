using AutoFixture;
using Corely.Security.KeyStore;
using Moq.Protected;

namespace Corely.UnitTests.Security.KeyStore
{
    public class FileAsymmetricKeyStoreProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly FileAsymmetricKeyStoreProvider _fileKeyStoreProvider;
        private readonly string _filePublicKey;
        private readonly string _filePrivateKey;

        public FileAsymmetricKeyStoreProviderTests()
        {
            _filePublicKey = _fixture.Create<string>();
            _filePrivateKey = _fixture.Create<string>();
            var fileKeyStoreProvider = new Mock<FileAsymmetricKeyStoreProvider>(_fixture.Create<string>());
            fileKeyStoreProvider.Protected()
                .Setup<(string, string)>("GetFileContents")
                .Returns(() => (_filePublicKey, _filePrivateKey));
            _fileKeyStoreProvider = fileKeyStoreProvider.Object;
        }

        [Fact]
        public void GetCurrentKey_ReturnsKey()
        {
            var (publicKey, privateKey) = _fileKeyStoreProvider.GetCurrentKeys();
            Assert.Equal(_filePublicKey, publicKey);
            Assert.Equal(_filePrivateKey, privateKey);
        }

        [Fact]
        public void GetCurrentVersion_ReturnsVersion1()
        {
            var version = _fileKeyStoreProvider.GetCurrentVersion();
            Assert.Equal(1, version);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_ReturnsVersion1_WithAnyVersion(int version)
        {
            var (publicKey, privateKey) = _fileKeyStoreProvider.Get(version);
            Assert.Equal(_filePublicKey, publicKey);
            Assert.Equal(_filePrivateKey, privateKey);
        }
    }
}
