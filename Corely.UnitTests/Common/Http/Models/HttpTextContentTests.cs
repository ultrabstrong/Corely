using Corely.Common.Http.Models;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Common.Http.Models
{
    public class HttpTextContentTests
    {
        private readonly HttpTextContent _httpTextContent = new("content");

        [Fact]
        public void HttpFormUrlEncodedContent_IsOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<string>>(_httpTextContent);
        }

        [Fact]
        public void HttpTextContent_IsOfTypeHttpStringContentBase()
        {
            Assert.IsAssignableFrom<HttpStringContentBase>(_httpTextContent);
        }

        [Fact]
        public void HttpTextContent_SetsContent_OnConstruction()
        {
            Assert.Equal("content", _httpTextContent.Content);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void HttpTextContent_AllowsEmptyContent(string content)
        {
            var httpTextContent = new HttpTextContent(content);
            Assert.Equal(content, httpTextContent.Content);
        }
    }
}
