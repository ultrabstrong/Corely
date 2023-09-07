using Corely.Shared.Providers.Http.Builders;
using Serilog;

namespace Corely.Shared.Providers.Http
{
    public sealed class HttpProxyProvider : HttpProxyProviderBase
    {
        public HttpProxyProvider(
            ILogger logger,
            IHttpContentBuilder httpContentBuilder,
            string host)
            : base(logger, httpContentBuilder, host)
        {
        }
    }
}
