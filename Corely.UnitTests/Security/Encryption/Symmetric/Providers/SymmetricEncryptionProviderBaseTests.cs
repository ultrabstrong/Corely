using Corely.Security.Encryption;
using Corely.Security.Encryption.Providers;
using Corely.Security.KeyStore.Symmetric;

namespace Corely.UnitTests.Security.Encryption.Symmetric.Providers
{
    public class SymmetricEncryptionProviderBaseTests : SymmetricEncryptionProviderGenericTests
    {
        private class MockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => TEST_ENCRYPTION_TYPE_CODE;
            public MockEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => $"{Guid.NewGuid()}{value}";
            protected override string DecryptInternal(string value, string key) => value[36..];
        }

        private class NullMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => null!;
            public NullMockEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class EmptyMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => string.Empty;

            public EmptyMockEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }

            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class WhitespaceMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => " ";
            public WhitespaceMockEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private class ColonMockEncryptionProvider : SymmetricEncryptionProviderBase
        {
            public override string EncryptionTypeCode => "as:df";
            public ColonMockEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
                : base(keyStoreProvider) { }
            protected override string EncryptInternal(string value, string key) => value;
            protected override string DecryptInternal(string value, string key) => value;
        }

        private const string TEST_ENCRYPTION_TYPE_CODE = "00";

        private readonly MockEncryptionProvider _mockEncryptionProvider = new(new Mock<ISymmetricKeyStoreProvider>().Object);

        [Fact]
        public void NullEncryptionTypeCode_ShouldThrowArgumentNullException_OnBuild()
        {
            var ex = Record.Exception(() => new NullMockEncryptionProvider(new Mock<ISymmetricKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void EmptyEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new EmptyMockEncryptionProvider(new Mock<ISymmetricKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void WhitespaceEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new WhitespaceMockEncryptionProvider(new Mock<ISymmetricKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void ColonEncryptionTypeCode_ShouldThrowArgumentException_OnBuild()
        {
            var ex = Record.Exception(() => new ColonMockEncryptionProvider(new Mock<ISymmetricKeyStoreProvider>().Object));
            Assert.NotNull(ex);
            Assert.IsType<EncryptionException>(ex);
        }

        [Fact]
        public override void EncryptionTypeCode_ShouldReturnCorrectCode_ForImplementation()
        {
            Assert.Equal(TEST_ENCRYPTION_TYPE_CODE, _mockEncryptionProvider.EncryptionTypeCode);
        }

        public override ISymmetricEncryptionProvider GetEncryptionProvider(ISymmetricKeyStoreProvider keyStoreProvider)
        {
            return new MockEncryptionProvider(keyStoreProvider);
        }
    }
}
