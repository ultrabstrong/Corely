using AutoFixture;
using Corely.IAM.Mappers.AutoMapper.TypeConverters;
using Corely.Security.Encryption.Factories;
using Corely.Security.Encryption.Providers;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.TypeConverters
{
    public class SymmetricEncryptedStringToEncryptedValueTypeConverterTests
    {
        private readonly SymmetricEncryptedStringToEncryptedValueTypeConverter _converter;
        private readonly Fixture _fixture = new();

        public SymmetricEncryptedStringToEncryptedValueTypeConverterTests()
        {
            var encryptionProvider = Mock.Of<ISymmetricEncryptionProvider>();
            var encryptionProviderFactory = Mock.Of<ISymmetricEncryptionProviderFactory>(
                f => f.GetProviderForDecrypting(It.IsAny<string>()) == encryptionProvider);

            _converter = new(encryptionProviderFactory);
        }

        [Fact]
        public void Convert_ShouldReturnEncryptedValue()
        {
            var value = _fixture.Create<string>();

            var result = _converter.Convert(value, default, default);

            Assert.Equal(value, result.Secret);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Convert_ShouldReturnNullEmptyOrWhitespace(string value)
        {
            var result = _converter.Convert(value, default, default);

            Assert.Equal(value, result.Secret);
        }
    }
}
