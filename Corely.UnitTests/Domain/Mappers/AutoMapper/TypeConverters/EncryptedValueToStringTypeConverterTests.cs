using AutoFixture;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Encryption;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.TypeConverters
{
    public class EncryptedValueToStringTypeConverterTests
    {
        private readonly EncryptedValueToStringTypeConverter _converter = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void Convert_ShouldReturnString()
        {
            var value = _fixture.Create<string>();
            var encryptedValue = new EncryptedValue(Mock.Of<IEncryptionProvider>())
            { Secret = value };

            var result = _converter.Convert(encryptedValue, default, default);

            Assert.Equal(value, result);
        }

        [Fact]
        public void Convert_ShouldReturnNull_WithNullSecretValue()
        {
            var result = _converter.Convert(null, default, default);

            Assert.Null(result);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Convert_ShouldReturnNullEmptyOrWhitespace(string value)
        {
            var encryptedValue = new EncryptedValue(Mock.Of<IEncryptionProvider>())
            { Secret = value };

            var result = _converter.Convert(encryptedValue, default, default);

            Assert.Equal(value, result);
        }
    }
}
