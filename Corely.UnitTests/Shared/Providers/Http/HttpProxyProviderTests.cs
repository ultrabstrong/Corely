using Corely.Shared.Providers.Http;
using Corely.Shared.Providers.Http.Builders;

namespace Corely.UnitTests.Shared.Providers.Http
{
    public class HttpProxyProviderTests
    {
        private readonly HttpProxyProvider _httpProxyProvider;

        public HttpProxyProviderTests()
        {
            _httpProxyProvider = new HttpProxyProvider(new Mock<IHttpContentBuilder>().Object, "http://localhost/");
        }

        [Fact]
        public void HttpProxyProvider_ShouldImplementIHttpProxyProvider()
        {
            Assert.IsAssignableFrom<IHttpProxyProvider>(_httpProxyProvider);
        }

        [Fact]
        public void HttpProxyProvider_ShouldImplementIDisposable()
        {
            Assert.IsAssignableFrom<IDisposable>(_httpProxyProvider);
        }

        [Fact]
        public void HttpProxyProvider_ShouldImplementHttpProxyProviderBase()
        {
            Assert.IsAssignableFrom<HttpProxyProviderBase>(_httpProxyProvider);
        }
    }
}
