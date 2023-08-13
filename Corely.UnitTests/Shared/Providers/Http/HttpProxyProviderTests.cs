﻿using Corely.Shared.Providers.Http;
using Corely.Shared.Providers.Http.Builders;

namespace Corely.UnitTests.Shared.Providers.Http
{
    public class HttpProxyProviderTests
    {
        private readonly HttpProxyProvider _httpProxyProvider;

        public HttpProxyProviderTests()
        {
            _httpProxyProvider = new HttpProxyProvider(new Mock<IHttpContentBuilder>().Object);
        }
    }
}