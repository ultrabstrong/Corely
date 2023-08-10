using Corely.Shared.Extensions;

namespace Corely.UnitTests.Shared.Extensions
{
    public class UrlEncodingExtensionsTests
    {
        [Fact]
        public void UrlEncode_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UrlEncodingExtensions.UrlEncode(null));
        }

        [Fact]
        public void UrlEncode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, UrlEncodingExtensions.UrlEncode(string.Empty));
        }

        [Fact]
        public void UrlDecode_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UrlEncodingExtensions.UrlDecode(null));
        }

        [Fact]
        public void UrlDecode_EmptyString_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, UrlEncodingExtensions.UrlDecode(string.Empty));
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
            var encoded = UrlEncodingExtensions.UrlEncode(source);
            var decoded = UrlEncodingExtensions.UrlDecode(encoded);
            Assert.Equal(source, decoded);
        }
    }
}
