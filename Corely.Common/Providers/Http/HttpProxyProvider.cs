using Corely.Common.Providers.Http.Builders;
using Serilog;

namespace Corely.Common.Providers.Http
{
    public sealed class HttpProxyProvider(
        ILogger logger,
        IHttpContentBuilder httpContentBuilder,
        string host) : HttpProxyProviderBase(logger, httpContentBuilder, host)
    {
    }
}
