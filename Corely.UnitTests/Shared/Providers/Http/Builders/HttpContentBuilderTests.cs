using Corely.Shared.Providers.Http.Builders;
using Corely.Shared.Providers.Http.Models;

namespace Corely.UnitTests.Shared.Providers.Http.Builders
{
    public class HttpContentBuilderTests
    {
        private readonly HttpContentBuilder _builder = new();

        [Fact]
        public void HttpContentBuilder_ShouldImplementIHttpContentBuilder()
        {
            Assert.IsAssignableFrom<IHttpContentBuilder>(_builder);
        }

        [Fact]
        public void HttpContentBuilder_Build_ShouldThrowArgumentNullException()
        {
            void act() => _builder.Build(null as IHttpContent<string>);
            Assert.Throws<ArgumentNullException>(act);
        }

        private class TestHttpContent : IHttpContent<string>
        {
            public string Content { get; } = null;
        }

        [Fact]
        public void HttpContentBuilder_Build_ShouldThrowNotImplementedException()
        {
            void act() => _builder.Build(new TestHttpContent());
            Assert.Throws<NotImplementedException>(act);
        }

        [Fact]
        public void HttpContentBuilder_Build_ShouldReturnMultipartFormDataContent()
        {
            var content = new HttpMultipartFormDataContent(new() { { "key1", "value1" } });
            var result = _builder.Build(content);
            Assert.IsType<MultipartFormDataContent>(result);
            Assert.Equal("multipart/form-data", result.Headers.ContentType?.MediaType);
            var resultContent = "Content-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=key1\r\n\r\nvalue1";
            Assert.Contains(resultContent, result.ReadAsStringAsync().Result);
        }

        [Fact]
        public void HttpContentBuilder_Build_ShouldReturnFormUrlEncodedContent()
        {
            var content = new HttpFormUrlEncodedContent(new() { { "key1", "value1" } });
            var result = _builder.Build(content);
            Assert.IsType<FormUrlEncodedContent>(result);
            Assert.Equal("application/x-www-form-urlencoded", result.Headers.ContentType?.MediaType);
            Assert.Equal("key1=value1", result.ReadAsStringAsync().Result);
        }

        [Fact]
        public void HttpContentBuilder_Build_ShouldReturnJsonContent()
        {
            var content = new HttpJsonContent(new { Test = "test" });
            var result = _builder.Build(content);
            Assert.IsType<StringContent>(result);
            Assert.Equal("application/json", result.Headers.ContentType?.MediaType);
            Assert.Equal("{\"Test\":\"test\"}", result.ReadAsStringAsync().Result);
        }

        [Fact]
        public void HttpContentBuilder_Build_ShouldReturnTextContent()
        {
            var content = new HttpTextContent("test");
            var result = _builder.Build(content);
            Assert.IsType<StringContent>(result);
            Assert.Equal("text/plain", result.Headers.ContentType?.MediaType);
            Assert.Equal("test", result.ReadAsStringAsync().Result);
        }
    }
}
