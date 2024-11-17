using Corely.Common.Http.Models;

namespace Corely.UnitTests.Common.Http.Models
{
    public class HttpMultipartFormDataContentTests
    {
        private readonly HttpMultipartFormDataContent _httpMultipartFormDataContent = new([]);

        [Fact]
        public void HttpMultipartFormDataContent_IsOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<Dictionary<string, string>>>(_httpMultipartFormDataContent);
        }

        [Fact]
        public void HttpMultipartFormDataContent_IsOfTypeHttpDictionaryContentBase()
        {
            Assert.IsAssignableFrom<HttpDictionaryContentBase>(_httpMultipartFormDataContent);
        }

        [Fact]
        public void HttpMultipartFormDataContent_Throws_WhenContentIsNull()
        {
            var ex = Record.Exception(() => new HttpMultipartFormDataContent(null));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }


        [Theory, MemberData(nameof(HttpMultipartFormDataContentTestData))]
        public void HttpMultipartFormDataContent_ReturnsExpectedContent(Dictionary<string, string> content)
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
