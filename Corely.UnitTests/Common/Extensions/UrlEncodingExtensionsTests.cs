using Corely.Common.Extensions;

namespace Corely.UnitTests.Common.Extensions
{
    public class UrlEncodingExtensionsTests
    {
        [Fact]
        public void UrlEncode_Null_ThrowsArgumentNullException()
        {
            var ex = Record.Exception(() => UrlEncodingExtensions.UrlEncode(null));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void UrlEncode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal("", "".UrlEncode());
        }

        [Fact]
        public void UrlDecode_Null_ThrowsArgumentNullException()
        {
            var ex = Record.Exception(() => UrlEncodingExtensions.UrlDecode(null));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Fact]
        public void UrlDecode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal("", "".UrlDecode());
        }

        [Theory, MemberData(nameof(GetUrlEncodeDecodeTestData))]
        public void UrlEncodeThenDecode_ShouldReturnOriginalString(string source)
        {
            Assert.Equal(source, source.UrlEncode().UrlDecode());
        }
        public static IEnumerable<object[]> GetUrlEncodeDecodeTestData()
        {
            yield return new object[] { "http://www.google.com" };
            yield return new object[] { "http://www.google.com?query=hello world" };
            yield return new object[] { "http://www.google.com?query=hello+world" };
            yield return new object[] { "http://www.google.com?query=hello%20world" };
            yield return new object[] { "http://www.google.com?query=hello%2Bworld" };
            yield return new object[] { "http://www.google.com?query=hello%2520world" };
            yield return new object[] { "http://www.google.com?query=hello%252Bworld" };
            yield return new object[] { "http://www.google.com?query=hello%252520world" };
        }

        [Fact]
        public void UrlEncode_ShouldEncodeSpecialCharacters()
        {
            Assert.Equal("%21%40%23%24%25%5E%26%2A%28%29_%2B%20", "!@#$%^&*()_+ ".UrlEncode());
        }

        [Fact]
        public void UrlDecode_ShouldDecodeSpecialCharacters()
        {
            Assert.Equal("!@#$%^&*()_+ ", "%21%40%23%24%25%5E%26%2A%28%29_%2B%20".UrlDecode());
        }
    }
}
