using AutoFixture;
using Corely.IAM.Mappers.AutoMapper.TypeConverters;
using Corely.Security.Encryption.Models;
using Corely.Security.Encryption.Providers;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.TypeConverters
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
