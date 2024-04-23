using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore;

namespace Corely.UnitTests.Security.Encryption.Providers
{
    public class EncryptionProviderBaseTests : EncryptionProviderGenericTests
    {
        private class MockEncryptionProvider : EncryptionProviderBase
        {
            public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            public MockEncryptionProvider(IKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
            protected override string DecryptInternal(string value, string key) => value[36..];
        }

        private class NullMockEncryptionProvider : EncryptionProviderBase
        {
            public override string EncryptionTypeCode => null!;
            public NullMockEncryptionProvider(IKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class EmptyMockEncryptionProvider : EncryptionProviderBase
        {
            public override string EncryptionTypeCode => "";

            public EmptyMockEncryptionProvider(IKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }

            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class WhitespaceMockEncryptionProvider : EncryptionProviderBase
        {
            public override string EncryptionTypeCode => " ";
            public WhitespaceMockEncryptionProvider(IKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class ColonMockEncryptionProvider : EncryptionProviderBase
        {
            public override string EncryptionTypeCode => "as:df";
            public ColonMockEncryptionProvider(IKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockEncryptionProvider _mockEncryptionProvider = new(new Mock<IKeyStoreProvider>().Object);

        [Fact]
        public void NullEncryptionTypeCode_ShouldThrowArgumentNullException_OnBuild()
        {
            var ex = Record.Exception(() => new NullMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void EmptyEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new EmptyMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void WhitespaceEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new WhitespaceMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ColonEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new ColonMockEncryptionProvider(new Mock<IKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
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
