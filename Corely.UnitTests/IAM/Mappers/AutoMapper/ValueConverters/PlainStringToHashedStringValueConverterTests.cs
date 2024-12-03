using AutoFixture;
using Corely.IAM.Mappers.AutoMapper.ValueConverters;
using Corely.Security.Hashing.Factories;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.IAM.Mappers.AutoMapper.ValueConverters;

public class PlainStringToHashedStringValueConverterTests
{
    private readonly Fixture _fixture = new();
    private readonly PlainStringToHashedStringValueConverter _valueConverter;

    public PlainStringToHashedStringValueConverterTests()
    {
        var serviceFactory = new ServiceFactory();
        _valueConverter = new(serviceFactory.GetRequiredService<IHashProviderFactory>());
    }

    [Fact]
    public void Convert_ReturnsHashedValue()
    {
        var value = _fixture.Create<string>();

        var result = _valueConverter.Convert(value, default);

        Assert.NotNull(result.Hash);
        Assert.True(result.Verify(value));
    }

    [Theory, ClassData(typeof(EmptyAndWhitespace))]
    public void Convert_ReturnsHashedValue_WithEmptyWhitespace(string value)
    {
        var result = _valueConverter.Convert(value, default);

        Assert.NotNull(result.Hash);
        Assert.True(result.Verify(value));
    }

    [Fact]
    public void Convert_Throws_WhenValueIsNull()
    {
        var ex = Record.Exception(() => _valueConverter.Convert(null, default));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }
}
