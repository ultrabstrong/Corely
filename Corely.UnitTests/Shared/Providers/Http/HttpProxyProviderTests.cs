using Corely.Common.Providers.Http;
using Corely.Common.Providers.Http.Builders;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Serilog;

namespace Corely.UnitTests.Shared.Providers.Http
{
    [Collection(nameof(CollectionNames.SerilogCollection))]
    public class HttpProxyProviderTests
    {
        private readonly ILogger _logger;
        private readonly HttpProxyProvider _httpProxyProvider;

        public HttpProxyProviderTests(TestLoggerFixture loggerFixture)
        {
            _logger = loggerFixture.Logger.ForContext<HttpProxyProviderTests>();

            _httpProxyProvider = new HttpProxyProvider(
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
