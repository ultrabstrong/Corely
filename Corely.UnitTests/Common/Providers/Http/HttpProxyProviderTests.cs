using AutoFixture;
using Corely.Common.Providers.Http;
using Corely.Common.Providers.Http.Builders;
using Corely.Common.Providers.Http.Models;
using Corely.UnitTests.AB.TestBase;
using Corely.UnitTests.Collections;
using Corely.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Corely.UnitTests.Common.Providers.Http
{
    [Collection(nameof(CollectionNames.ServiceFactory))]
    public class HttpProxyProviderTests
    {
        private readonly ILogger<HttpProxyProvider> _logger;
        private readonly HttpProxyProvider _httpProxyProvider;
        private readonly Fixture _fixture = new();

        private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;

        public HttpProxyProviderTests(ServiceFactory serviceFactory)
        {
            _logger = serviceFactory.GetRequiredService<ILogger<HttpProxyProvider>>();

            _httpProxyProvider = new(
                _logger,
                new Mock<IHttpContentBuilder>().Object,
                "http://localhost/");

            var httpClientMock = new Mock<HttpClient>();
            httpClientMock.Setup(c =>
                c.SendAsync(
                    It.IsAny<HttpRequestMessage>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => new HttpResponseMessage(_httpStatusCode));

            NonPublicHelpers.SetNonPublicField(_httpProxyProvider, "_httpClient",
                httpClientMock.Object);
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

        [Fact]
        public void HttpProxyProvider_ShouldDispose()
        {
            _httpProxyProvider.Dispose();
            var disposed = NonPublicHelpers.GetNonPublicField<bool>(_httpProxyProvider, "_disposed");
            Assert.True(disposed);
        }

        [Fact]
        public async Task SendRequestForHttpResponse_ShouldSendRequestForHttpResponse()
        {
            var requestUri = _fixture.Create<string>();
            var request = new HttpSendRequest(requestUri, HttpMethod.Get);
            var httpContent = new HttpStringContentBase(_fixture.Create<string>());
            var response = await _httpProxyProvider.SendRequestForHttpResponse(request, httpContent);

            Assert.Equal(_httpStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task SendRequestForHttpResponse_ShouldThrowForBadRequest()
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            var requestUri = _fixture.Create<string>();
            var request = new HttpSendRequest(requestUri, HttpMethod.Get);

            var ex = await Record.ExceptionAsync(async () => await _httpProxyProvider.SendRequestForHttpResponse<string>(request));

            Assert.NotNull(ex);
            Assert.IsType<HttpRequestException>(ex);
        }
    }
}
