using Corely.Common.Providers.Http.Builders;
using Microsoft.Extensions.Logging;

namespace Corely.Common.Providers.Http
{
    public sealed class HttpProxyProvider : HttpProxyProviderBase
    {
        public HttpProxyProvider(
            ILogger<HttpProxyProvider> logger,
            IHttpContentBuilder httpContentBuilder,
            string host)
            : base(logger, httpContentBuilder, host) { }
    }
}
