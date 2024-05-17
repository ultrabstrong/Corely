using AutoFixture;
using Corely.Security.KeyStore.Symmetric;
using Moq.Protected;

namespace Corely.UnitTests.Security.KeyStore.Symmetric
{
    public class FileSymmetricKeyStoreProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly FileSymmetricKeyStoreProvider _fileKeyStoreProvider;
        private readonly string _fileKey;

        public FileSymmetricKeyStoreProviderTests()
        {
            _fileKey = _fixture.Create<string>();
            var fileKeyStoreProvider = new Mock<FileSymmetricKeyStoreProvider>(_fixture.Create<string>());
            fileKeyStoreProvider.Protected()
                .Setup<string>("GetFileContents")
                .Returns(() => _fileKey);
            _fileKeyStoreProvider = fileKeyStoreProvider.Object;
        }

        [Fact]
        public void GetCurrentKey_ReturnsKey()
        {
            var key = _fileKeyStoreProvider.GetCurrentKey();
            Assert.Equal(_fileKey, key);
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
            var key = _fileKeyStoreProvider.Get(version);
            Assert.Equal(_fileKey, key);
        }
    }
}
