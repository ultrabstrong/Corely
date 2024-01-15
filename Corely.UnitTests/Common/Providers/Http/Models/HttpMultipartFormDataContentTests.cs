using Corely.Common.Providers.Http.Models;

namespace Corely.UnitTests.Common.Providers.Http.Models
{
    public class HttpMultipartFormDataContentTests
    {
        private readonly HttpMultipartFormDataContent _httpMultipartFormDataContent = new([]);

        [Fact]
        public void HttpMultipartFormDataContent_ShouldBeOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<Dictionary<string, string>>>(_httpMultipartFormDataContent);
        }

        [Fact]
        public void HttpMultipartFormDataContent_ShouldBeOfTypeHttpDictionaryContentBase()
        {
            Assert.IsAssignableFrom<HttpDictionaryContentBase>(_httpMultipartFormDataContent);
        }

        [Fact]
        public void HttpMultipartFormDataContent_ShouldThrowArgumentNullException_WhenContentIsNull()
        {
            var exception = Record.Exception(() => new HttpMultipartFormDataContent(null));
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }


        [Theory, MemberData(nameof(HttpMultipartFormDataContentTestData))]
        public void HttpMultipartFormDataContent_ShouldReturnExpectedContent(Dictionary<string, string> content)
        {
            var httpFormUrlEncodedContent = new HttpFormUrlEncodedContent(content);

            Assert.Equal(content, httpFormUrlEncodedContent.Content);
        }

        public static IEnumerable<object[]> HttpMultipartFormDataContentTestData() =>
        [
            [new Dictionary<string, string> { }],
            [new Dictionary<string, string> { { "key", "value" } }],
            [new Dictionary<string, string> { { "key", "value" }, { "key2", "value2" } }]
        ];
    }
}
