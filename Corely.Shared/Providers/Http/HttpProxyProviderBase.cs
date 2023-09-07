using Corely.Shared.Providers.Http.Builders;
using Corely.Shared.Providers.Http.Models;
using Serilog;

namespace Corely.Shared.Providers.Http
{
    public abstract class HttpProxyProviderBase : IHttpProxyProvider, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IHttpContentBuilder _httpContentBuilder;
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        public HttpProxyProviderBase(
            ILogger logger,
            IHttpContentBuilder httpContentBuilder,
            string host)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(httpContentBuilder, nameof(httpContentBuilder));
            ArgumentException.ThrowIfNullOrWhiteSpace(host, nameof(host));
            _logger = logger;
            _httpContentBuilder = httpContentBuilder;
            _httpClient = new HttpClient() { BaseAddress = new Uri(host) };
        }

        public async virtual Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
            HttpSendRequest request,
            IHttpContent<T>? httpContent = null)
        {
            var requestMessage = CreateHttpRequestMessage(request, httpContent);

            _logger.ForContext(nameof(request.Headers), request.Headers)
                .ForContext(nameof(request.Parameters), request.Parameters)
                .Debug("Sending HTTP request {Uri}", requestMessage.RequestUri);

            HttpResponseMessage result = await _httpClient.SendAsync(requestMessage);

            if (result.IsSuccessStatusCode)
            {
                _logger.Debug("Http request succeeded");
                return result;
            }
            else
            {
                string body = await result.Content.ReadAsStringAsync();
                string message = $"{(int)result.StatusCode} {result.StatusCode} - {result.ReasonPhrase}{Environment.NewLine}{result.RequestMessage}";
                _logger.ForContext(nameof(body), body)
                    .Error("Http request failed : {Message}", message);
                throw new HttpRequestException(message, new Exception(body));
            }
        }

        private HttpRequestMessage CreateHttpRequestMessage<T>(HttpSendRequest request, IHttpContent<T>? httpContent = null)
        {
            HttpRequestMessage message = request.CreateHttpRequestMessage();

            if (httpContent != null)
            {
                message.Content = _httpContentBuilder.Build(httpContent);
            }

            return message;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _httpClient?.Dispose();
                    }
                    catch { }
                }
                _disposed = true;
            }
        }
    }
}
