using Corely.Shared.Providers.Http.Builders;
using Corely.Shared.Providers.Http.Models;

namespace Corely.Shared.Providers.Http
{
    public abstract class HttpProxyProviderBase : IHttpProxyProvider, IDisposable
    {
        private readonly IHttpContentBuilder _httpContentBuilder;
        private HttpClient _httpClient;
        private bool _disposed = false;

        public bool IsConnected { get; internal set; }

        public HttpProxyProviderBase(IHttpContentBuilder httpContentBuilder)
        {
            ArgumentNullException.ThrowIfNull(httpContentBuilder, nameof(httpContentBuilder));
            _httpContentBuilder = httpContentBuilder;
        }

        public virtual void Connect(string host)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(host, nameof(host));
            if (!IsConnected)
            {
                try
                {
                    _httpClient = new HttpClient() { BaseAddress = new Uri(host) };
                    IsConnected = true;
                }
                catch
                {
                    IsConnected = false;
                    throw;
                }
            }
        }

        public virtual void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                _httpClient?.Dispose();
                _httpClient = null;
            }
        }

        public async virtual Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
            HttpSendRequest request,
            IHttpContent<T>? httpContent = null)
        {
            if (!IsConnected) { throw new Exception("proxy is not connected"); }

            HttpResponseMessage result = await _httpClient.SendAsync(CreateHttpRequestMessage(request, httpContent));

            if (result.IsSuccessStatusCode)
            {
                return result;
            }
            else
            {
                string body = await result.Content.ReadAsStringAsync();
                throw new HttpRequestException($"{(int)result.StatusCode} {result.StatusCode} - {result.ReasonPhrase}{Environment.NewLine}{result.RequestMessage}", new Exception(body));
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
                    try { Disconnect(); }
                    catch { }
                }
                _disposed = true;
            }
        }
    }
}
