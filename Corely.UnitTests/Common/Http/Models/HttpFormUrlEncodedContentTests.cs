using Corely.Common.Http.Models;

namespace Corely.UnitTests.Common.Http.Models
{
    public class HttpFormUrlEncodedContentTests
    {
        private readonly HttpFormUrlEncodedContent _httpFormUrlEncodedContent = new([]);

        [Fact]
        public void HttpFormUrlEncodedContent_IsOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<Dictionary<string, string>>>(_httpFormUrlEncodedContent);
        }

        [Fact]
        public void HttpFormUrlEncodedContent_IsOfTypeHttpDictionaryContentBase()
        {
            Assert.IsAssignableFrom<HttpDictionaryContentBase>(_httpFormUrlEncodedContent);
        }

        [Fact]
        public void HttpFormUrlEncodedContent_Throws_WhenContentIsNull()
        {
            var ex = Record.Exception(() => new HttpFormUrlEncodedContent(null));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        [Theory, MemberData(nameof(HttpFormUrlEncodedContentTestData))]
        public void HttpFormUrlEncodedContent_ReturnsExpectedContent(Dictionary<string, string> content)
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
