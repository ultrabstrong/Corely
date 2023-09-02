using Corely.Shared.Providers.Http.Builders;
using Corely.Shared.Providers.Http.Models;

namespace Corely.Shared.Providers.Http
{
    public abstract class HttpProxyProviderBase : IHttpProxyProvider, IDisposable
    {
        private readonly IHttpContentBuilder _httpContentBuilder;
        private readonly HttpClient _httpClient;
        private bool _disposed = false;

        public HttpProxyProviderBase(IHttpContentBuilder httpContentBuilder, string host)
        {
            ArgumentNullException.ThrowIfNull(httpContentBuilder, nameof(httpContentBuilder));
            ArgumentException.ThrowIfNullOrWhiteSpace(host, nameof(host));
            _httpContentBuilder = httpContentBuilder;
            _httpClient = new HttpClient() { BaseAddress = new Uri(host) };
        }

        public async virtual Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
            HttpSendRequest request,
            IHttpContent<T>? httpContent = null)
        {
            HttpResponseMessage result = await _httpClient.SendAsync(CreateHttpRequestMessage(request, httpContent));

            if (result.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                string body = await result.Content.ReadAsStringAsync();
                string message = $"{(int)result.StatusCode} {result.StatusCode} - {result.ReasonPhrase}{Environment.NewLine}{result.RequestMessage}";
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
