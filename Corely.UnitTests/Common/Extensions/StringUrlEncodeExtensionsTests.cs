using Corely.Common.Extensions;

namespace Corely.UnitTests.Common.Extensions
{
    public class StringUrlEncodeExtensionsTests
    {
        [Fact]
        public void UrlEncode_Null_ThrowsArgumentNullException()
        {
            var ex = Record.Exception(() => StringUrlEncodeExtensions.UrlEncode(null));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void UrlEncode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, string.Empty.UrlEncode());
        }

        [Fact]
        public void UrlDecode_ThrowsArgumentNullException_WithNullInput()
        {
            var ex = Record.Exception(() => StringUrlEncodeExtensions.UrlDecode(null));
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
}
