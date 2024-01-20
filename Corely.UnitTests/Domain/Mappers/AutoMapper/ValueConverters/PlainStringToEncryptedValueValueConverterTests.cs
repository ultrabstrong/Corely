using AutoFixture;
using Corely.Common.Providers.Security.Factories;
using Corely.Domain.Mappers.AutoMapper.ValueConverters;
using Corely.UnitTests.ClassData;
using Corely.UnitTests.Collections;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.ValueConverters
{
    [Collection(CollectionNames.ServiceFactory)]
    public class PlainStringToEncryptedValueValueConverterTests
    {
        private readonly Fixture _fixture = new();
        private readonly PlainStringToEncryptedValueValueConverter _converter;

        public PlainStringToEncryptedValueValueConverterTests(ServiceFactory serviceFactory)
        {
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
