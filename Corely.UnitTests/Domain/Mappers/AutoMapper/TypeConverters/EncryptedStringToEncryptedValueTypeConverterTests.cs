using AutoFixture;
using Corely.Common.Providers.Security.Encryption;
using Corely.Common.Providers.Security.Factories;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.TypeConverters
{
    public class EncryptedStringToEncryptedValueTypeConverterTests
    {
        private readonly EncryptedStringToEncryptedValueTypeConverter _converter;
        private readonly Fixture _fixture = new();

        public EncryptedStringToEncryptedValueTypeConverterTests()
        {
            var encryptionProvider = Mock.Of<IEncryptionProvider>();
            var encryptionProviderFactory = Mock.Of<IEncryptionProviderFactory>(
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
