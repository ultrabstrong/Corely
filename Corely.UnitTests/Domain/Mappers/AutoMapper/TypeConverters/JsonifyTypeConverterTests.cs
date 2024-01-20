using AutoFixture;
using Corely.Domain.Mappers.AutoMapper.TypeConverters;
using System.Text.Json;

namespace Corely.UnitTests.Domain.Mappers.AutoMapper.TypeConverters
{
    public class JsonifyTypeConverterTests
    {
        private class TestClass
        {
            public string? Name { get; set; }
        }

        private readonly JsonifyTypeConverter<TestClass> _converter = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public void Convert_ShouldReturnJson()
        {
            var value = _fixture.Create<TestClass>();

            var result = _converter.Convert(value, default, default);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(value), result);
        }

        [Fact]
        public void Convert_ShouldReturnJson_WithNullSource()
        {
            TestClass? value = null;

            var result = _converter.Convert(null, default, default);

            Assert.NotNull(result);
            Assert.Equal(JsonSerializer.Serialize(value), result);
        }
    }
}
