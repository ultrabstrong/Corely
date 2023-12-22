using AutoFixture;
using Corely.Shared.Providers.Security.Secrets;
using Moq.Protected;

namespace Corely.UnitTests.Shared.Providers.Security.Secrets
{
    public class FileSecretProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly FileSecretProvider _fileSecretProvider;
        private readonly string _fileSecret;

        public FileSecretProviderTests()
        {
            _fileSecret = _fixture.Create<string>();
            var fileSecretProvider = new Mock<FileSecretProvider>(_fixture.Create<string>());
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
