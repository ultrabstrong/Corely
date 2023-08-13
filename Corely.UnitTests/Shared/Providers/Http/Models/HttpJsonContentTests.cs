using Corely.Shared.Providers.Http.Models;
using Corely.UnitTests.ClassData;

namespace Corely.UnitTests.Shared.Providers.Http.Models
{
    public class HttpJsonContentTests
    {
        private readonly HttpJsonContent _httpJsonContent;

        public HttpJsonContentTests()
        {
            _httpJsonContent = new HttpJsonContent("content");
        }

        [Fact]
        public void HttpFormUrlEncodedContent_ShouldBeOfTypeIHttpContent()
        {
            Assert.IsAssignableFrom<IHttpContent<string>>(_httpJsonContent);
        }

        [Fact]
        public void HttpJsonContent_ShouldBeOfTypeHttpStringContentBase()
        {
            Assert.IsAssignableFrom<HttpStringContentBase>(_httpJsonContent);
        }

        [Fact]
        public void HttpJsonContent_ShouldSetContent_OnConstruction()
        {
            Assert.Equal("content", _httpJsonContent.Content);
        }

        [Theory, ClassData(typeof(NullEmptyAndWhitespace))]
        public void HttpJsonContent_ShouldAllowEmptyContent(string content)
        {
            var httpJsonContent = new HttpJsonContent(content);
            Assert.Equal(content, httpJsonContent.Content);
        }

        [Theory, MemberData(nameof(HttpJsonContentTestData))]
        public void HttpJsonContent_ShouldSerializeContent(object content, string expected)
        {
            var httpJsonContent = new HttpJsonContent(content);
            Assert.Equal(expected, httpJsonContent.Content);
        }

        public static IEnumerable<object[]> HttpJsonContentTestData()
        {
            yield return new object[] { new { Test = "test" }, "{\"Test\":\"test\"}" };
            yield return new object[] { new { Test = 1 }, "{\"Test\":1}" };
            yield return new object[] { new { Test = 1.1 }, "{\"Test\":1.1}" };
            yield return new object[] { new { Test = true }, "{\"Test\":true}" };
            yield return new object[] { new { Test = new { Test = "test" } }, "{\"Test\":{\"Test\":\"test\"}}" };
            yield return new object[] { new { Test = new { Test = 1 } }, "{\"Test\":{\"Test\":1}}" };
            yield return new object[] { new { Test = new { Test = 1.1 } }, "{\"Test\":{\"Test\":1.1}}" };
            yield return new object[] { new { Test = new { Test = true } }, "{\"Test\":{\"Test\":true}}" };
            yield return new object[] { new object[] { "test" }, "[\"test\"]" };
            yield return new object[] { new object[] { 1 }, "[1]" };
            yield return new object[] { new object[] { 1.1 }, "[1.1]" };
            yield return new object[] { new object[] { true }, "[true]" };
            yield return new object[] { new object[] { new { Test = "test" } }, "[{\"Test\":\"test\"}]" };
            yield return new object[] { new object[] { new { Test = 1 } }, "[{\"Test\":1}]" };
            yield return new object[] { new object[] { new { Test = 1.1 } }, "[{\"Test\":1.1}]" };
            yield return new object[] { new object[] { new { Test = true } }, "[{\"Test\":true}]" };
            yield return new object[] { null, "null" };
            yield return new object[] { new { Test = (object?)null }, "{\"Test\":null}" };
            yield return new object[] { new { Test = new { Test = (object?)null } }, "{\"Test\":{\"Test\":null}}" };
        }
    }
}
