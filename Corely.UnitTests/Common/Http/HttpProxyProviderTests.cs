using AutoFixture;
using Corely.Common.Http;
using Corely.Common.Http.Builders;
using Corely.Common.Http.Models;
using Corely.UnitTests.AB.TestBase;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Corely.UnitTests.Common.Http
{
    public class HttpProxyProviderTests
    {
        private readonly ILogger<HttpProxyProvider> _logger;
        private readonly HttpProxyProvider _httpProxyProvider;
        private readonly Fixture _fixture = new();

        private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;

        public HttpProxyProviderTests()
        {
            var serviceFactory = new ServiceFactory();
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
        public void HttpProxyProvider_ImplementsIHttpProxyProvider()
        {
            Assert.IsAssignableFrom<IHttpProxyProvider>(_httpProxyProvider);
        }

        [Fact]
        public void HttpProxyProvider_ImplementsIDisposable()
        {
            Assert.IsAssignableFrom<IDisposable>(_httpProxyProvider);
        }

        [Fact]
        public void HttpProxyProvider_ImplementsHttpProxyProviderBase()
        {
            Assert.IsAssignableFrom<HttpProxyProviderBase>(_httpProxyProvider);
        }

        [Fact]
        public void Dispose_Disposes()
        {
            _httpProxyProvider.Dispose();
            var disposed = NonPublicHelpers.GetNonPublicField<bool>(_httpProxyProvider, "_disposed");
            Assert.True(disposed);
        }

        [Fact]
        public async Task SendRequestForHttpResponse_SendsRequestForHttpResponse()
        {
            var requestUri = _fixture.Create<string>();
            var request = new HttpSendRequest(requestUri, HttpMethod.Get);
            var httpContent = new HttpStringContentBase(_fixture.Create<string>());
            var response = await _httpProxyProvider.SendRequestForHttpResponse(request, httpContent);

            Assert.Equal(_httpStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task SendRequestForHttpResponse_ThrowsForBadRequest()
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
