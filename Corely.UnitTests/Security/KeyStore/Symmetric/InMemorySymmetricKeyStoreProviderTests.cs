using AutoFixture;
using Corely.Security.KeyStore;
using Corely.Security.KeyStore.Symmetric;

namespace Corely.UnitTests.Security.KeyStore.Symmetric
{
    public class InMemorySymmetricKeyStoreProviderTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void GetCurrentKey_ShouldReturnKey()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

            var currentKey = keyStoreProvider.GetCurrentKey();

            Assert.Equal(key, currentKey);
        }

        [Fact]
        public void GetCurrentVersion_ShouldReturnOne()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

            var currentVersion = keyStoreProvider.GetCurrentVersion();

            Assert.Equal(1, currentVersion);
        }

        [Fact]
        public void Add_ShouldIncrementVersion()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(key);

            keyStoreProvider.Add(key);

            var currentKey = keyStoreProvider.GetCurrentKey();
            var currentVersion = keyStoreProvider.GetCurrentVersion();

            Assert.Equal(key, currentKey);
            Assert.Equal(2, currentVersion);
        }

        [Fact]
        public void Get_ShouldReturnKey()
        {
            var key = _fixture.Create<string>();
            var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(_fixture.Create<string>());

            keyStoreProvider.Add(key);
            keyStoreProvider.Add(_fixture.Create<string>());

            var keyForVersion = keyStoreProvider.Get(2);

            Assert.Equal(key, keyForVersion);
        }

        [Fact]
        public void Get_ShouldThrowException_WhenVersionIsInvalid()
        {
            var keyStoreProvider = new InMemorySymmetricKeyStoreProvider(_fixture.Create<string>());

            var ex = Record.Exception(() => keyStoreProvider.Get(2));

            Assert.NotNull(ex);
            Assert.IsType<KeyStoreException>(ex);
        }
    }
}
