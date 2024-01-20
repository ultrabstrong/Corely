using AutoFixture;
using Corely.Common.Providers.Security.Factories;
using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.TypeConverters
{
    public class HashedStringToHashedValueTypeConverterTests
    {
        private readonly HashStringToHashedValueTypeConverter _converter;
        private readonly Fixture _fixture = new();

        public HashedStringToHashedValueTypeConverterTests()
        {
            var hashProvider = Mock.Of<IHashProvider>();
            var hashProviderFactory = Mock.Of<IHashProviderFactory>(
                f => f.GetProviderToVerify(It.IsAny<string>()) == hashProvider);

            _converter = new(hashProviderFactory);
        }

        [Fact]
        public void Convert_ShouldReturnHashedValue()
        {
            var value = _fixture.Create<string>();

            var result = _converter.Convert(value, default, default);

            Assert.Equal(value, result.Hash);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Convert_ShouldReturnNullEmptyOrWhitespace(string value)
        {
            var result = _converter.Convert(value, default, default);

            Assert.Equal(value, result.Hash);
        }
    }
}
