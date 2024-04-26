using AutoFixture;
using Corely.IAM.Mappers.AutoMapper.ValueConverters;
using Corely.Security.Encryption.Factories;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.ValueConverters
{
    public class PlainStringToEncryptedValueValueConverterTests
    {
        private readonly Fixture _fixture = new();
        private readonly PlainStringToEncryptedValueValueConverter _converter;

        public PlainStringToEncryptedValueValueConverterTests()
        {
            var serviceFactory = new ServiceFactory();
            _converter = new(serviceFactory.GetRequiredService<IEncryptionProviderFactory>());
        }

        [Fact]
        public void Convert_ShouldReturnEncryptedValue()
        {
            var value = _fixture.Create<string>();

            var result = _converter.Convert(value, default);

            Assert.NotNull(result.Secret);
            Assert.Equal(value, result.GetDecrypted());
        }

        [Theory, ClassData(typeof(EmptyAndWhitespace))]
        public void Convert_ShouldReturnEncryptedValue_WithEmptyWhitespace(string value)
        {
            var result = _converter.Convert(value, default);

            Assert.NotNull(result.Secret);
            Assert.Equal(value, result.GetDecrypted());
        }

        [Fact]
        public void Convert_ShouldThrowArgumentNullException_WhenValueIsNull()
        {
            var ex = Record.Exception(() => _converter.Convert(null, default));

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
