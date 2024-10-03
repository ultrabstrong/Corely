using AutoFixture;
using Corely.Security.Keys;
using Corely.Security.KeyStore;
using Corely.Security.Signature.Providers;

namespace Corely.UnitTests.Security.Signature.Providers
{
    public abstract class SignatureProviderGenericTests
    {
        private readonly Fixture _fixture = new();
        private readonly IAsymmetricSignatureKeyProvider _keyProvider;
        private readonly InMemoryAsymmetricKeyStoreProvider _keyStoreProvider;
        private readonly ISignatureProvider _signatureProvider;

        public SignatureProviderGenericTests()
        {
            _keyProvider = GetKeyProvider();
            var (publicKey, privateKey) = _keyProvider.CreateKeys();
            _keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);
            _signatureProvider = GetSignatureProvider(publicKey);
        }

        [Fact]
        public void Sign_ReturnsAValue()
        {
            var data = _fixture.Create<string>();

            var signature = _signatureProvider.Sign(data, _keyStoreProvider);

            Assert.NotEmpty(signature);
            Assert.NotEqual(data, signature);
        }

        [Fact]
        public void Sign_ThrowsArgumentNullException_WithNullInput()
        {
            var ex = Record.Exception(() => _signatureProvider.Sign(null!, _keyStoreProvider));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void Sign_ThenVerify_ReturnsTrue()
        {
            var data = _fixture.Create<string>();
            var signature = _signatureProvider.Sign(data, _keyStoreProvider);

            var result = _signatureProvider.Verify(data, signature, _keyStoreProvider);

            Assert.True(result);
        }

        [Fact]
        public void Sign_ThenVerify_ReturnsFalse_WithDifferentData()
        {
            var data = _fixture.Create<string>();
            var signature = _signatureProvider.Sign(data, _keyStoreProvider);

            var result = _signatureProvider.Verify(_fixture.Create<string>(), signature, _keyStoreProvider);

            Assert.False(result);
        }

        [Fact]
        public void Sign_ThenVerify_ReturnsFalse_WithDifferentSignature()
        {
            var data = _fixture.Create<string>();
            var otherData = _fixture.Create<string>();
            var otherSignature = _signatureProvider.Sign(otherData, _keyStoreProvider);

            var result = _signatureProvider.Verify(data, otherSignature, _keyStoreProvider);

            Assert.False(result);
        }

        [Fact]
        public void Sign_ThenVerify_ReturnsFalse_WithDifferentKey()
        {
            var data = _fixture.Create<string>();
            var signature = _signatureProvider.Sign(data, _keyStoreProvider);
            var (publicKey, privateKey) = _keyProvider.CreateKeys();
            var keyStoreProvider = new InMemoryAsymmetricKeyStoreProvider(publicKey, privateKey);

            var result = _signatureProvider.Verify(data, signature, keyStoreProvider);

            Assert.False(result);
        }

        [Fact]
        public void Verify_ThrowsArgumentNullException_WithNullData()
        {
            var ex = Record.Exception(() => _signatureProvider.Verify(null!, _fixture.Create<string>(), _keyStoreProvider));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public abstract void SignatureTypeCode_ReturnsCorrectCode_ForImplementation();

        public abstract ISignatureProvider GetSignatureProvider(string expectedVerifyKey);

        public abstract IAsymmetricSignatureKeyProvider GetKeyProvider();
    }
}
