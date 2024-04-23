using AutoFixture;
using Corely.Security.KeyStore;
using Moq.Protected;

namespace Corely.UnitTests.Security.KeyStore
{
    public class FileKeyStoreProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly FileKeyStoreProvider _fileKeyStoreProvider;
        private readonly string _fileKey;

        public FileKeyStoreProviderTests()
        {
            _fileKey = _fixture.Create<string>();
            var fileKeyStoreProvider = new Mock<FileKeyStoreProvider>(_fixture.Create<string>());
            fileKeyStoreProvider.Protected()
                .Setup<string>("GetFileContents")
                .Returns(() => _fileKey);
            _fileKeyStoreProvider = fileKeyStoreProvider.Object;
        }

        [Fact]
        public void GetCurrentVersion_ShouldReturnVersion1()
        {
            var (key, version) = _fileKeyStoreProvider.GetCurrentVersion();
            Assert.Equal(_fileKey, key);
            Assert.Equal(1, version);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_ShouldReturnVersion1_WithAnyVersion(int version)
        {
            var key = _fileKeyStoreProvider.Get(version);
            Assert.Equal(_fileKey, key);
        }
    }
}
