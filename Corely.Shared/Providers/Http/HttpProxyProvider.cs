using Corely.Shared.Providers.Http.Builders;

namespace Corely.Shared.Providers.Http
{
    public sealed class HttpProxyProvider : HttpProxyProviderBase
    {
        public HttpProxyProvider(IHttpContentBuilder httpContentBuilder, string host)
            : base(httpContentBuilder, host)
        {
        }
    }
}
