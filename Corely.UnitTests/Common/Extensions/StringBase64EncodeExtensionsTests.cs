using Corely.Common.Extensions;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Extensions;

public class StringBase64EncodeExtensionsTests
{
    [Theory, ClassData(typeof(NullAndEmpty))]
    public void Base64Encode_ReturnsEmptyString_WhenStringIsNullOrEmpty(string s)
    {
        Assert.Equal(string.Empty, s.Base64Encode());
    }

    [Theory, ClassData(typeof(NullAndEmpty))]
    public void Base64Decode_ReturnsEmptyString_WhenStringIsNullOrEmpty(string s)
    {
        Assert.Equal(string.Empty, s.Base64Decode());
    }

    [Theory]
    [InlineData("test")]
    [InlineData("test string with spaces")]
    [InlineData("test string with spaces and special characters !@#$%^&*()_+")]
    public void Base64Encode_Base64DecodesToOriginalString(string s)
    {
        Assert.Equal(s, s.Base64Encode().Base64Decode());
    }

}
