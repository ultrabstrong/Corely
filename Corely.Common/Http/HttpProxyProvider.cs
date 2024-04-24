using Corely.Common.Http.Builders;
using Microsoft.Extensions.Logging;

namespace Corely.Common.Http
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
