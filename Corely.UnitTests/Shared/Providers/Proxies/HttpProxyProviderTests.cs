using Corely.Shared.Providers.Proxies;

namespace Corely.UnitTests.Shared.Providers.Proxies
{
    public class HttpProxyProviderTests
    {
        private readonly HttpProxyProvider _httpProxyProvider;

        public HttpProxyProviderTests()
        {
            _httpProxyProvider = new HttpProxyProvider();
        }
    }
}
