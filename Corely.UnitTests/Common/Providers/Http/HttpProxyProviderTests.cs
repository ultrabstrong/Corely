using Corely.Common.Providers.Http;
using Corely.Common.Providers.Http.Builders;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;

namespace Corely.UnitTests.Common.Providers.Http
{
    [Collection(nameof(CollectionNames.LoggerCollection))]
    public class HttpProxyProviderTests
    {
        private readonly ILogger<HttpProxyProvider> _logger;
        private readonly HttpProxyProvider _httpProxyProvider;

        public HttpProxyProviderTests(LoggerFixture loggerFixture)
        {
            _logger = loggerFixture.CreateLogger<HttpProxyProvider>();

            _httpProxyProvider = new(
                _logger,
                new Mock<IHttpContentBuilder>().Object,
                "http://localhost/");
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
