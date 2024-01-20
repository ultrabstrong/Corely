using AutoFixture;
using Corely.Common.Providers.Security.Factories;
using Corely.Domain.Mappers.AutoMapper.ValueConverters;
using Corely.UnitTests.ClassData;
using Corely.UnitTests.Collections;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.ValueConverters
{
    [Collection(CollectionNames.ServiceFactory)]
    public class PlainStringToHashedValueValueConverterTests
    {
        private readonly Fixture _fixture = new();
        private readonly PlainStringToHashedValueValueConverter _converter;

        public PlainStringToHashedValueValueConverterTests(ServiceFactory serviceFactory)
        {
            _converter = new(serviceFactory.GetRequiredService<IHashProviderFactory>());
        }

        [Fact]
        public void Convert_ShouldReturnHashedValue()
        {
            var value = _fixture.Create<string>();

            var result = _converter.Convert(value, default);

            Assert.NotNull(result.Hash);
            Assert.True(result.Verify(value));
        }

        [Theory, ClassData(typeof(EmptyAndWhitespace))]
        public void Convert_ShouldReturnHashedValue_WithEmptyWhitespace(string value)
        {
            var result = _converter.Convert(value, default);

            Assert.NotNull(result.Hash);
            Assert.True(result.Verify(value));
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
