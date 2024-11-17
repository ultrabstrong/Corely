using Corely.Common.Http.Builders;
using Corely.Common.Http.Models;

namespace Corely.UnitTests.Common.Http.Builders
{
    public class HttpContentBuilderTests
    {
        private readonly HttpContentBuilder _builder = new();

        [Fact]
        public void HttpContentBuilder_ImplementsIHttpContentBuilder()
        {
            Assert.IsAssignableFrom<IHttpContentBuilder>(_builder);
        }

        [Fact]
        public void Build_Throws_WithNullArg()
        {
            var ex = Record.Exception(() => _builder.Build((null as IHttpContent<string>)!));
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
        }

        private class TestHttpContent : IHttpContent<string>
        {
            public string Content { get; } = null;
        }

        [Fact]
        public void Build_Throws_WithUnimplementedContentType()
        {
            var ex = Record.Exception(() => _builder.Build(new TestHttpContent()));
            Assert.NotNull(ex);
            Assert.IsType<NotImplementedException>(ex);
        }

        [Fact]
        public async Task Build_ReturnsMultipartFormDataContent()
        {
            var content = new HttpMultipartFormDataContent(new() { { "key1", "value1" } });
            var result = _builder.Build(content);
            Assert.IsType<MultipartFormDataContent>(result);
            Assert.Equal("multipart/form-data", result.Headers.ContentType?.MediaType);
            var resultContent = "Content-Type: text/plain; charset=utf-8\r\nContent-Disposition: form-data; name=key1\r\n\r\nvalue1";
            Assert.Contains(resultContent, await result.ReadAsStringAsync());
        }

        [Fact]
        public async Task Build_ReturnsFormUrlEncodedContent()
        {
            var content = new HttpFormUrlEncodedContent(new() { { "key1", "value1" } });
            var result = _builder.Build(content);
            Assert.IsType<FormUrlEncodedContent>(result);
            Assert.Equal("application/x-www-form-urlencoded", result.Headers.ContentType?.MediaType);
            Assert.Equal("key1=value1", await result.ReadAsStringAsync());
        }

        [Fact]
        public async Task Build_ReturnsJsonContent()
        {
            var content = new HttpJsonContent(new { Test = "test" });
            var result = _builder.Build(content);
            Assert.IsType<StringContent>(result);
            Assert.Equal("application/json", result.Headers.ContentType?.MediaType);
            Assert.Equal("{\"Test\":\"test\"}", await result.ReadAsStringAsync());
        }

        [Fact]
        public async Task Build_ReturnsTextContent()
        {
            var content = new HttpTextContent("test");
            var result = _builder.Build(content);
            Assert.IsType<StringContent>(result);
            Assert.Equal("text/plain", result.Headers.ContentType?.MediaType);
            Assert.Equal("test", await result.ReadAsStringAsync());
        }
    }
}
