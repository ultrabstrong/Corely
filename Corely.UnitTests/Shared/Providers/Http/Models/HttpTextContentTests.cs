using Corely.Shared.Providers.Http.Models;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Http.Models
{
    public class HttpTextContentTests
    {
        private readonly HttpTextContent _httpTextContent = new("content");

        [Fact]
        public void HttpFormUrlEncodedContent_ShouldBeOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<string>>(_httpTextContent);
        }

        [Fact]
        public void HttpTextContent_ShouldBeOfTypeHttpStringContentBase()
        {
            Assert.IsAssignableFrom<HttpStringContentBase>(_httpTextContent);
        }

        [Fact]
        public void HttpTextContent_ShouldSetContent_OnConstruction()
        {
            Assert.Equal("content", _httpTextContent.Content);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void HttpTextContent_ShouldAllowEmptyContent(string content)
        {
            var httpTextContent = new HttpTextContent(content);
            Assert.Equal(content, httpTextContent.Content);
        }
    }
}
