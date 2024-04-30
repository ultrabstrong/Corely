﻿using AutoFixture;
using Corely.IAM.Mappers.AutoMapper.ValueConverters;
using Corely.Security.Hashing.Factories;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.ValueConverters
{
    public class PlainStringToHashedValueValueConverterTests
    {
        private readonly Fixture _fixture = new();
        private readonly PlainStringToHashedValueValueConverter _converter;

        public PlainStringToHashedValueValueConverterTests()
        {
            var serviceFactory = new ServiceFactory();
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

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenHashProviderIsNull()
        {
            static PlainStringToHashedValueValueConverter act() => new(null);

            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }
    }
}
