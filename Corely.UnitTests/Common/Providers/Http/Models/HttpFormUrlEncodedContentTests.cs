using Corely.Common.Providers.Http.Models;

namespace Corely.UnitTests.Common.Providers.Http.Models
{
    public class HttpFormUrlEncodedContentTests
    {
        private readonly HttpFormUrlEncodedContent _httpFormUrlEncodedContent = new([]);

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
            var exception = Record.Exception(() => new HttpFormUrlEncodedContent(null));
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory, MemberData(nameof(HttpFormUrlEncodedContentTestData))]
        public void HttpFormUrlEncodedContent_ShouldReturnExpectedContent(Dictionary<string, string> content)
        {
            var httpFormUrlEncodedContent = new HttpFormUrlEncodedContent(content);

            Assert.Equal(content, httpFormUrlEncodedContent.Content);
        }

        public static IEnumerable<object[]> HttpFormUrlEncodedContentTestData() =>
        [
            [new Dictionary<string, string> { }],
            [new Dictionary<string, string> { { "key", "value" } }],
            [new Dictionary<string, string> { { "key", "value" }, { "key2", "value2" } }]
        ];
    }
}
