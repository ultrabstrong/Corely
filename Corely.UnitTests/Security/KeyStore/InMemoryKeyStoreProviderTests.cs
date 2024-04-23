using AutoFixture;
using Corely.Security.KeyStore;

namespace Corely.UnitTests.Security.KeyStore
{
    public class InMemoryKeyStoreProviderTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void GetCurrentVersion_ShouldReturnOne()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemoryKeyStoreProvider(key);

            var (currentKey, currentVersion) = keyStoreProvider.GetCurrentVersion();

            Assert.Equal(key, currentKey);
            Assert.Equal(1, currentVersion);
        }

        [Fact]
        public void Add_ShouldIncrementVersion()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemoryKeyStoreProvider(key);

            keyStoreProvider.Add(key);

            var (currentKey, currentVersion) = keyStoreProvider.GetCurrentVersion();

            Assert.Equal(key, currentKey);
            Assert.Equal(2, currentVersion);
        }

        [Fact]
        public void Get_ShouldReturnKey()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemoryKeyStoreProvider(_fixture.Create<string>());

            keyStoreProvider.Add(key);
            keyStoreProvider.Add(_fixture.Create<string>());

            var keyForVersion = keyStoreProvider.Get(2);

            Assert.Equal(key, keyForVersion);
        }

        [Fact]
        public void Get_ShouldThrowException_WhenVersionIsInvalid()
        {
            var keyStoreProvider = new InMemoryKeyStoreProvider(_fixture.Create<string>());

            var ex = Record.Exception(() => keyStoreProvider.Get(2));

            Assert.NotNull(ex);
            Assert.IsType<KeyStoreException>(ex);
        }
    }
}
