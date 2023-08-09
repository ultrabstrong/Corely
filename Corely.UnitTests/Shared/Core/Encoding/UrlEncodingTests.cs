using Corely.Shared.Core.Encoding;

namespace Corely.UnitTests.Shared.Core.Encoding
{
    public class UrlEncodingTests
    {
        [Fact]
        public void UrlEncode_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UrlEncoding.UrlEncode(null));
        }

        [Fact]
        public void UrlEncode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, UrlEncoding.UrlEncode(string.Empty));
        }

        [Fact]
        public void UrlDecode_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UrlEncoding.UrlDecode(null));
        }

        [Fact]
        public void UrlDecode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, UrlEncoding.UrlDecode(string.Empty));
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

        [Theory, MemberData(nameof(GetUrlEncodeDecodeTestData))]
        public void UrlEncodeThenDecode_ShouldReturnOriginalString(string source)
        {
            var encoded = UrlEncoding.UrlEncode(source);
            var decoded = UrlEncoding.UrlDecode(encoded);
            Assert.Equal(source, decoded);
        }
    }
}
