using Corely.Common.Providers.Http.Builders;
using Serilog;

namespace Corely.Common.Providers.Http
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
