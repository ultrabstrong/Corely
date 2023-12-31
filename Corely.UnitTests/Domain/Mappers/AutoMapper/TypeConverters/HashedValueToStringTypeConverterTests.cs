using AutoFixture;
using Corely.Common.Models.Security;
using Corely.Common.Providers.Security.Hashing;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.TypeConverters
{
    public class HashedValueToStringTypeConverterTests
    {
        private readonly HashedValueToStringTypeConverter _converter = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void Convert_ShouldReturnString()
        {
            var value = _fixture.Create<string>();
            var hashedValue = new HashedValue(Mock.Of<IHashProvider>())
            { Hash = value };

            var result = _converter.Convert(hashedValue, default, default);

            Assert.Equal(value, result);
        }

        [Fact]
        public void Convert_ShouldReturnNull_WithNullHashValue()
        {
            var result = _converter.Convert(null, default, default);

            Assert.Null(result);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void Convert_ShouldReturnNullEmptyOrWhitespace(string value)
        {
            var hashedValue = new HashedValue(Mock.Of<IHashProvider>())
            { Hash = value };

            var result = _converter.Convert(hashedValue, default, default);

            Assert.Equal(value, result);
        }
    }
}
