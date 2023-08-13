using Corely.Shared.Providers.Http.Builders;

namespace Corely.Shared.Providers.Http
{
    public class HttpProxyProvider : HttpProxyProviderBase
    {
        public HttpProxyProvider(IHttpContentBuilder httpContentBuilder)
            : base(httpContentBuilder)
        {
        }
    }
}
