using Corely.Shared.Providers.Http.Models;

namespace Corely.UnitTests.Shared.Providers.Http.Models
{
    public class HttpFormUrlEncodedContentTests
    {
        private readonly HttpFormUrlEncodedContent _httpFormUrlEncodedContent;

        public HttpFormUrlEncodedContentTests()
        {
            _httpFormUrlEncodedContent = new HttpFormUrlEncodedContent(new());
        }

        [Fact]
        public void HttpFormUrlEncodedContent_ShouldBeOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<Dictionary<string, string>>>(_httpFormUrlEncodedContent);
        }

        [Fact]
        public void HttpFormUrlEncodedContent_ShouldBeOfTypeHttpDictionaryContentBase()
        {
            Assert.IsAssignableFrom<HttpDictionaryContentBase>(_httpFormUrlEncodedContent);
        }

        [Fact]
        public void HttpFormUrlEncodedContent_ShouldThrowArgumentNullException_WhenContentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpFormUrlEncodedContent(null));
        }

        [Theory, MemberData(nameof(HttpFormUrlEncodedContentTestData))]
        public void HttpFormUrlEncodedContent_ShouldReturnExpectedContent(Dictionary<string, string> content)
        {
            var httpFormUrlEncodedContent = new HttpFormUrlEncodedContent(content);

            Assert.Equal(content, httpFormUrlEncodedContent.Content);
        }

        public static IEnumerable<object[]> HttpFormUrlEncodedContentTestData()
        {
            yield return new object[] { new Dictionary<string, string> { } };
            yield return new object[] { new Dictionary<string, string> { { "key", "value" } } };
            yield return new object[] { new Dictionary<string, string> { { "key", "value" }, { "key2", "value2" } } };
        }
    }
}
