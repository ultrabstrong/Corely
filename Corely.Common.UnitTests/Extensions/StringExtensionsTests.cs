using Corely.Common.Extensions;
using Corely.TestBase.ClassData;

namespace Corely.Common.UnitTests.Extensions;

public class StringExtensionsTests
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

    [Fact]
    public void UrlEncode_Null_Throws()
    {
        var ex = Record.Exception(() => StringExtensions.UrlEncode(null));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void UrlEncode_EmptyString_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, string.Empty.UrlEncode());
    }

    [Fact]
    public void UrlDecode_Throws_WithNullInput()
    {
        var ex = Record.Exception(() => StringExtensions.UrlDecode(null));
        Assert.NotNull(ex);
        Assert.IsType<ArgumentNullException>(ex);
    }

    [Fact]
    public void UrlDecode_EmptyString_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, string.Empty.UrlDecode());
    }

    [Theory, MemberData(nameof(GetUrlEncodeDecodeTestData))]
    public void UrlEncodeThenDecode_ReturnsOriginalString(string source)
    {
        Assert.Equal(source, source.UrlEncode().UrlDecode());
    }
    public static IEnumerable<object[]> GetUrlEncodeDecodeTestData() =>
    [
        ["http://www.google.com"],
            ["http://www.google.com?query=hello world"],
            ["http://www.google.com?query=hello+world"],
            ["http://www.google.com?query=hello%20world"],
            ["http://www.google.com?query=hello%2Bworld"],
            ["http://www.google.com?query=hello%2520world"],
            ["http://www.google.com?query=hello%252Bworld"],
            ["http://www.google.com?query=hello%252520world"]
    ];

    [Fact]
    public void UrlEncode_EncodesSpecialCharacters()
    {
        Assert.Equal("%21%40%23%24%25%5E%26%2A%28%29_%2B%20", "!@#$%^&*()_+ ".UrlEncode());
    }

    [Fact]
    public void UrlDecode_DecodesSpecialCharacters()
    {
        Assert.Equal("!@#$%^&*()_+ ", "%21%40%23%24%25%5E%26%2A%28%29_%2B%20".UrlDecode());
    }
}
