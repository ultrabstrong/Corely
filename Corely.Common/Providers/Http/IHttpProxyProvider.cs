using Corely.Common.Providers.Http.Models;

namespace Corely.Common.Providers.Http
{
    public interface IHttpProxyProvider
    {
        Task<HttpResponseMessage> SendRequestForHttpResponse<T>(
            HttpSendRequest request,
            IHttpContent<T>? httpContent = null);
    }
}
