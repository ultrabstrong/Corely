using Corely.Common.Providers.Http.Builders;
using Microsoft.Extensions.Logging;

namespace Corely.Common.Providers.Http
{
    public sealed class HttpProxyProvider(
        ILogger<HttpProxyProvider> logger,
        IHttpContentBuilder httpContentBuilder,
        string host)
        : HttpProxyProviderBase(logger, httpContentBuilder, host)
    {
    }
}
