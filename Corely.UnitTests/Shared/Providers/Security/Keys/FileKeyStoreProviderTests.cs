using AutoFixture;
using Corely.Shared.Providers.Security.Keys;
using Moq.Protected;

namespace Corely.UnitTests.Shared.Providers.Security.Keys
{
    public class FileKeyStoreProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly FileKeyStoreProvider _fileSecretProvider;
        private readonly string _fileSecret;

        public FileKeyStoreProviderTests()
        {
            _fileSecret = _fixture.Create<string>();
            var fileSecretProvider = new Mock<FileKeyStoreProvider>(_fixture.Create<string>());
            fileSecretProvider.Protected()
                .Setup<string>("GetFileContents")
                .Returns(() => _fileSecret);
            _fileSecretProvider = fileSecretProvider.Object;
        }

        [Fact]
        public void GetCurrentVersion_ShouldReturnVersion1()
        {
            var (secret, version) = _fileSecretProvider.GetCurrentVersion();
            Assert.Equal(_fileSecret, secret);
            Assert.Equal(1, version);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_ShouldReturnVersion1_WithAnyVersion(int version)
        {
            var secret = _fileSecretProvider.Get(version);
            Assert.Equal(_fileSecret, secret);
        }
    }
}
