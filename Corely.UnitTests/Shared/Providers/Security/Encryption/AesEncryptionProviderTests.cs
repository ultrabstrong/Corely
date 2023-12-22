using AutoFixture;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Keys;
using Corely.UnitTests.AB.TestBase;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Security.Encryption
{
    public class AesEncryptionProviderTests
    {
        private readonly Fixture _fixture = new();
        private readonly AesKeyProvider _keyProvider = new();
        private readonly InMemoryKeyStoreProvider _secretProvider;
        private readonly AesEncryptionProvider _aesEncryptionProvider;

        public AesEncryptionProviderTests()
        {
            _secretProvider = new(_keyProvider.CreateKey());
            _aesEncryptionProvider = new(_secretProvider);
        }

        [Fact]
        public void EncryptionTypeCode_ShouldReturnAes()
        {
            var encryptionTypeCode = NonPublicHelpers.GetNonPublicProperty<string>(
                _aesEncryptionProvider, "TwoDigitEncryptionTypeCode");
            Assert.Equal(EncryptionProviderConstants.Aes, encryptionTypeCode);
        }

        [Fact]
        public void Encrypt_ShouldReturnEncryptedString()
        {
            var decrypted = _fixture.Create<string>();
            var encrypted = _aesEncryptionProvider.Encrypt(decrypted);
            Assert.NotEqual(decrypted, encrypted);
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Encrypt_ShouldThrow_WithNullOrEmpty(string decryptedString)
        {
            var exception = Record.Exception(() => _aesEncryptionProvider.Encrypt(decryptedString));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Fact]
        public void Encrypt_ShouldProduceDifferentEncryptedStrings()
        {
            var decrypted = _fixture.Create<string>();
            var encrypted1 = _aesEncryptionProvider.Encrypt(decrypted);
            var encrypted2 = _aesEncryptionProvider.Encrypt(decrypted);
            Assert.NotEqual(encrypted1, encrypted2);
        }

        [Fact]
        public void Decrypt_ShouldReturnDecryptedString()
        {
            var originalDecrypted = _fixture.Create<string>();
            var encrypted = _aesEncryptionProvider.Encrypt(originalDecrypted);
            var decrypted = _aesEncryptionProvider.Decrypt(encrypted);
            Assert.Equal(originalDecrypted, decrypted);
        }

        [Theory, ClassData(typeof(NullAndEmpty))]
        public void Decrypt_ShouldThrow_WithNullOrEmpty(string encryptedString)
        {
            var exception = Record.Exception(() => _aesEncryptionProvider.Encrypt(encryptedString));
            Assert.True(exception is ArgumentNullException || exception is ArgumentException);
        }

        [Fact]
        public void Decrypt_ShouldProduceSameStringThatWasEncrypted()
        {
            var decrpyted = _fixture.Create<string>();
            var encrypted1 = _aesEncryptionProvider.Encrypt(decrpyted);
            var encrypted2 = _aesEncryptionProvider.Encrypt(decrpyted);
            var decrypted1 = _aesEncryptionProvider.Decrypt(encrypted1);
            var decrypted2 = _aesEncryptionProvider.Decrypt(encrypted2);
            Assert.Equal(decrpyted, decrypted1);
            Assert.Equal(decrpyted, decrypted2);
        }

        [Fact]
        public void Decrypt_ShouldSucceed_AfterKeyIsUpdated()
        {
            var decrypted = _fixture.Create<string>();
            var encrypted = _aesEncryptionProvider.Encrypt(decrypted);

            _secretProvider.Add(_keyProvider.CreateKey());

            var reDecrypted = _aesEncryptionProvider.Decrypt(encrypted);
            Assert.Equal(decrypted, reDecrypted);
        }

        [Fact]
        public void ReEncryptWithCurrentKey_ShouldReturnSameValue_WithNoOptionsUsed()
        {
            var decrpyted = _fixture.Create<string>();
            var encrypted = _aesEncryptionProvider.Encrypt(decrpyted);
            var reEncrypted = _aesEncryptionProvider.ReEncryptWithCurrentKey(encrypted);
            var reDecrypted = _aesEncryptionProvider.Decrypt(reEncrypted);

            Assert.Equal(encrypted, reEncrypted);
            Assert.Equal(decrpyted, reDecrypted);
        }

        [Fact]
        public void ReEncryptWithCurrentKey_ShouldReturnDifferentValue_WithSkipIfAlreadyCurrentTrue()
        {
            var decrpyted = _fixture.Create<string>();
            var encrypted = _aesEncryptionProvider.Encrypt(decrpyted);
            var reEncrypted = _aesEncryptionProvider.ReEncryptWithCurrentKey(encrypted, false);
            var reDecrypted = _aesEncryptionProvider.Decrypt(reEncrypted);

            var startsWith = $"{EncryptionProviderConstants.Aes}{_secretProvider.GetCurrentVersion().Item2}:";

            Assert.StartsWith(startsWith, encrypted);
            Assert.StartsWith(startsWith, reEncrypted);
            Assert.NotEqual(encrypted, reEncrypted);
            Assert.Equal(decrpyted, reDecrypted);
        }

        [Fact]
        public void ReEncryptWithCurrentKey_ShouldReEncrypt_WithDifferentKey()
        {
            var decrpyted = _fixture.Create<string>();
            var encrypted = _aesEncryptionProvider.Encrypt(decrpyted);
            var firstStartsWith = $"{EncryptionProviderConstants.Aes}{_secretProvider.GetCurrentVersion().Item2}:";

            _secretProvider.Add(_keyProvider.CreateKey());

            var reEncrypted = _aesEncryptionProvider.ReEncryptWithCurrentKey(encrypted);
            var reDecrypted = _aesEncryptionProvider.Decrypt(reEncrypted);
            var secondStartsWith = $"{EncryptionProviderConstants.Aes}{_secretProvider.GetCurrentVersion().Item2}:";

            Assert.StartsWith(firstStartsWith, encrypted);
            Assert.StartsWith(secondStartsWith, reEncrypted);
            Assert.NotEqual(encrypted, reEncrypted);
            Assert.Equal(decrpyted, reDecrypted);
        }
    }
}
