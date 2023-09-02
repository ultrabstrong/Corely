using Corely.Shared.Providers.Http.Models;

namespace Corely.UnitTests.Shared.Providers.Http.Models
{
    public class HttpMultipartFormDataContentTests
    {
        private readonly HttpMultipartFormDataContent _httpMultipartFormDataContent = new(new());

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
            void act() => new HttpMultipartFormDataContent(null);
            Assert.Throws<ArgumentNullException>(act);
        }


        [Theory, MemberData(nameof(HttpMultipartFormDataContentTestData))]
        public void HttpMultipartFormDataContent_ShouldReturnExpectedContent(Dictionary<string, string> content)
        {
            var httpFormUrlEncodedContent = new HttpFormUrlEncodedContent(content);

            Assert.Equal(content, httpFormUrlEncodedContent.Content);
        }

        public static IEnumerable<object[]> HttpMultipartFormDataContentTestData()
        {
            yield return new object[] { new Dictionary<string, string> { } };
            yield return new object[] { new Dictionary<string, string> { { "key", "value" } } };
            yield return new object[] { new Dictionary<string, string> { { "key", "value" }, { "key2", "value2" } } };
        }

    }
}
