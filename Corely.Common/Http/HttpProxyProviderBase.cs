using Corely.Common.Extensions;
using Corely.Common.Http.Builders;
using Corely.Common.Http.Models;
using Corely.Common.Models;
using Microsoft.Extensions.Logging;

namespace Corely.Common.Http;

public abstract class HttpProxyProviderBase : DisposeBase, IHttpProxyProvider
{
    private readonly ILogger<HttpProxyProviderBase> _logger;
    private readonly IHttpContentBuilder _httpContentBuilder;
    private readonly HttpClient _httpClient;

    public HttpProxyProviderBase(
        ILogger<HttpProxyProviderBase> logger,
        IHttpContentBuilder httpContentBuilder,
        string host)
    {
        _logger = logger.ThrowIfNull(nameof(logger));

        _httpContentBuilder = httpContentBuilder
            .ThrowIfNull(nameof(httpContentBuilder));

        ArgumentException.ThrowIfNullOrWhiteSpace(host, nameof(host));
        _httpClient = new HttpClient() { BaseAddress = new Uri(host) };
    }

    public async virtual Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
        HttpSendRequest request,
        IHttpContent<T>? httpContent = null)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var requestMessage = CreateHttpRequestMessage(request, httpContent);

        using (_logger.BeginScope(new Dictionary<string, object> {
                { nameof(request.Headers), request.Headers },
                { nameof(request.Parameters), request.Parameters } }))
        {
            _logger.LogDebug("Sending HTTP request {Uri}", requestMessage.RequestUri);
        }

        HttpResponseMessage result = await _httpClient.SendAsync(requestMessage, CancellationToken.None);

        if (result.IsSuccessStatusCode)
        {
            _logger.LogDebug("Http request succeeded");
            return result;
        }
        else
        {
            string body = await result.Content.ReadAsStringAsync();
            string message = $"{(int)result.StatusCode} {result.StatusCode} - {result.ReasonPhrase}{Environment.NewLine}{result.RequestMessage}";

            using (_logger.BeginScope(new Dictionary<string, object> {
                    { nameof(body), body } }))
            {
                _logger.LogError("Http request failed : {Message}", message);
            }

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

    protected override void DisposeManagedResources() => _httpClient?.Dispose();
}
