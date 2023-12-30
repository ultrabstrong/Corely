using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Exceptions;
using Corely.Common.Providers.Security.Keys;

namespace Corely.UnitTests.Common.Providers.Security.Encryption
{
    public class EncryptionProviderBaseTests : EncryptionProviderGenericTests
    {
        private class MockEncryptionProvider(
            IKeyStoreProvider keyStoreProvider)
            : EncryptionProviderBase(keyStoreProvider)
        {
            public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;

            protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
            protected override string DecryptInternal(string value, string key) => value[36..];
        }

        private class NullMockEncryptionProvider(
            IKeyStoreProvider keyStoreProvider)
            : EncryptionProviderBase(keyStoreProvider)
        {
            public override string EncryptionTypeCode => null!;

            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class EmptyMockEncryptionProvider(
            IKeyStoreProvider keyStoreProvider)
            : EncryptionProviderBase(keyStoreProvider)
        {
            public override string EncryptionTypeCode => "";

            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class WhitespaceMockEncryptionProvider(
            IKeyStoreProvider keyStoreProvider)
            : EncryptionProviderBase(keyStoreProvider)
        {
            public override string EncryptionTypeCode => " ";

            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class ColonMockEncryptionProvider(
            IKeyStoreProvider keyStoreProvider)
            : EncryptionProviderBase(keyStoreProvider)
        {
            public override string EncryptionTypeCode => "as:df";

            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockEncryptionProvider _mockEncryptionProvider = new(new Mock<IKeyStoreProvider>().Object);

        [Fact]
        public void NullEncryptionTypeCode_ShouldThrowArgumentNullException_OnBuild()
        {
            var exception = Record.Exception(() => new NullMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void EmptyEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var exception = Record.Exception(() => new EmptyMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void WhitespaceEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var exception = Record.Exception(() => new WhitespaceMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void ColonEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var exception = Record.Exception(() => new ColonMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(exception);
            Assert.IsType<EncryptionProviderException>(exception);
        }

        [Fact]
        public override void EncryptionTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(TEST_ENCRYPTION_TYPE_CODE, _mockEncryptionProvider.EncryptionTypeCode);
        }

        public override IEncryptionProvider GetEncryptionProvider(IKeyStoreProvider keyStoreProvider)
        {
            return new MockEncryptionProvider(keyStoreProvider);
        }
    }
}
