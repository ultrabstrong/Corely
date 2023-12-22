using AutoFixture;
using Corely.Shared.Providers.Security.Secrets;

namespace Corely.UnitTests.Shared.Providers.Security.Secrets
{
    public class InMemorySecretProviderTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void GetCurrentVersion_ShouldReturnOne()
        {
            var secret = _fixture.Create<string>();
            var secretProvider = new InMemorySecretProvider(secret);

            var (currentSecret, currentVersion) = secretProvider.GetCurrentVersion();

            Assert.Equal(secret, currentSecret);
            Assert.Equal(1, currentVersion);
        }

        [Fact]
        public void Add_ShouldIncrementVersion()
        {
            var secret = _fixture.Create<string>();
            var secretProvider = new InMemorySecretProvider(secret);

            secretProvider.Add(secret);

            var (currentSecret, currentVersion) = secretProvider.GetCurrentVersion();

            Assert.Equal(secret, currentSecret);
            Assert.Equal(2, currentVersion);
        }

        [Fact]
        public void Get_ShouldReturnSecret()
        {
            var secret = _fixture.Create<string>();
            var secretProvider = new InMemorySecretProvider(_fixture.Create<string>());

            secretProvider.Add(secret);
            secretProvider.Add(_fixture.Create<string>());

            var secretForVersion = secretProvider.Get(2);

            Assert.Equal(secret, secretForVersion);
        }
    }
}
